using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace HarptosCalendarManager
{
    static class Utility
    {
        static string currentFilePath = null;

        public static void Clear()
        {
            currentFilePath = null;
        }

        public static void SaveAs(CalendarContents calendarToSave)
        {
            string temp = currentFilePath;
            currentFilePath = null;
            Save(calendarToSave);

            if (currentFilePath == null)
                currentFilePath = temp;
        }

        public static void AutoSave(CalendarContents calendarToSave)
        {
            if (currentFilePath == null)
                return;
            else
                Save(calendarToSave);
        }

        public static void Save(CalendarContents calendarToSave)
        {
            System.IO.Stream outStream;
            System.IO.StreamWriter writer = null;
            if (currentFilePath == null || currentFilePath == "") // if new file, open a dialog box
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Harptos Calendar files (*.hcal)|*.hcal|All files (*.*)|*.*";
                saveFileDialog1.InitialDirectory = Application.StartupPath;
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.DefaultExt = ".hcal";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((outStream = saveFileDialog1.OpenFile()) != null)
                    {  
                        writer = new System.IO.StreamWriter(outStream);
                        currentFilePath = saveFileDialog1.FileName;
                    }
                    else
                        return;
                }
                else
                    return;
            }

            else // save with no dialog
            {
                try
                {
                    writer = new System.IO.StreamWriter(currentFilePath);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error saving. Use Save As..");
                    currentFilePath = null;
                }
                
            }

                // Prevents serializing nested objects forever
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            writer.WriteLine(JsonConvert.SerializeObject(calendarToSave, settings));

            #region OLD FILE FORMAT
            /*
            int generalNoteCount = calendarToSave.GeneralNoteList.Count;
            writer.WriteLine(generalNoteCount);
            foreach (Note n in calendarToSave.GeneralNoteList)
            {
                writer.WriteLine(n.Date);
                writer.WriteLine((int)n.Importance);
                writer.WriteLine(n.NoteContent);
            }
            int campaignCount = calendarToSave.CampaignList.Count;
            if (campaignCount != 0)
            {
                writer.WriteLine(campaignCount);
                for (int campIndex = 0; campIndex < campaignCount; campIndex++)
                {
                    writer.WriteLine(calendarToSave.CampaignList[campIndex].Name);
                    writer.WriteLine(calendarToSave.CampaignList[campIndex].Tag);
                    writer.WriteLine(calendarToSave.CampaignList[campIndex].CurrentDate);

                    int timerCount = calendarToSave.CampaignList[campIndex].timers.Count();
                    writer.WriteLine(timerCount);
                    for (int timerIndex = 0; timerIndex < timerCount; timerIndex++)
                    {
                        writer.WriteLine(calendarToSave.CampaignList[campIndex].timers[timerIndex].message);
                        writer.WriteLine(calendarToSave.CampaignList[campIndex].timers[timerIndex].keepTrack);
                        writer.WriteLine(calendarToSave.CampaignList[campIndex].timers[timerIndex].returnDateString());
                    }

                    int noteCount = calendarToSave.CampaignList[campIndex].notes.Count();
                    writer.WriteLine(noteCount);
                    for (int noteIndex = 0; noteIndex < noteCount; noteIndex++)
                    {
                        writer.WriteLine(calendarToSave.CampaignList[campIndex].notes[noteIndex].Date);
                        writer.WriteLine((int)calendarToSave.CampaignList[campIndex].notes[noteIndex].Importance);
                        writer.WriteLine(calendarToSave.CampaignList[campIndex].notes[noteIndex].NoteContent);
                    }
                }
            }*/
            #endregion

            writer.Close();
        }

        public static CalendarContents Load()
        {
            CalendarContents loadedCalendar = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "Harptos Calendar files (*.hcal)|*.hcal|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Load(openFileDialog1.FileName);
                currentFilePath = openFileDialog1.FileName;

            }

            return loadedCalendar;
        }

        public static CalendarContents Load(string filename)
        {
            CalendarContents loadedCalendar = null;
            try
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(filename);
                loadedCalendar = ReadJSON(sr.ReadToEnd());
                currentFilePath = filename;
                sr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file. Original error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return loadedCalendar;
        }

        public static CalendarContents ReadJSON(string json)
        {
            try
            {
                return new CalendarContents(JsonConvert.DeserializeObject(json));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        #region OLD FILE FORMAT
        public static CalendarContents ReadInFile(System.IO.StreamReader sr)
        {
            CalendarContents loadedCalendar = new CalendarContents();
            int numOfGenNotes;
            if (Int32.TryParse(sr.ReadLine(), out numOfGenNotes))
            {
                for (int i = 0; i < numOfGenNotes; i++)
                    loadedCalendar.AddNote(ReadInNote(sr));

                int numOfCampaigns;
                if (Int32.TryParse(sr.ReadLine(), out numOfCampaigns))
                {
                    //StartLoadingBar(numOfCampaigns);
                    for (int i = 0; i < numOfCampaigns; i++)
                    {
                        Campaign loadedCampaign = new Campaign();

                        loadedCampaign.Name = sr.ReadLine();
                        loadedCampaign.Tag = sr.ReadLine();
                        loadedCampaign.CurrentDate = sr.ReadLine();

                        int numOfTimers = Int32.Parse(sr.ReadLine());
                        for (int j = 0; j < numOfTimers; j++)
                        {
                            string timerMessage = sr.ReadLine();
                            bool trackTimer = Convert.ToBoolean(sr.ReadLine());
                            string timerDate = sr.ReadLine();
                            loadedCampaign.addTimer(new Timer(timerDate, trackTimer, timerMessage));
                        }

                        int numOfNotes = Int32.Parse(sr.ReadLine());
                        for (int j = 0; j < numOfNotes; j++)
                            loadedCampaign.addNote(ReadInNote(sr));

                        loadedCalendar.AddCampaign(loadedCampaign);
                    }
                }
            }
            return loadedCalendar;
        }

        public static CalendarContents ReadInFile(System.IO.StreamReader sr, bool oldFile)
        {
            if (oldFile)
            {
                CalendarContents loadedCalendar = new CalendarContents();
                int numOfGenNotes;
                if (Int32.TryParse(sr.ReadLine(), out numOfGenNotes))
                {
                    for (int i = 0; i < numOfGenNotes; i++)
                        loadedCalendar.AddNote(ReadInNote(sr));

                    int numOfCampaigns;
                    if (Int32.TryParse(sr.ReadLine(), out numOfCampaigns))
                    {
                        //StartLoadingBar(numOfCampaigns);
                        for (int i = 0; i < numOfCampaigns; i++)
                        {
                            Campaign loadedCampaign = new Campaign();

                            loadedCampaign.Name = sr.ReadLine();
                            loadedCampaign.Tag = sr.ReadLine();
                            loadedCampaign.CurrentDate = sr.ReadLine();

                            int numOfTimers = Int32.Parse(sr.ReadLine());
                            for (int j = 0; j < numOfTimers; j++)
                            {
                                string timerMessage = sr.ReadLine();
                                bool trackTimer = Convert.ToBoolean(sr.ReadLine());
                                string timerDate = sr.ReadLine();
                                loadedCampaign.addTimer(new Timer(timerDate, trackTimer, timerMessage));
                            }

                            int numOfNotes = Int32.Parse(sr.ReadLine());
                            for (int j = 0; j < numOfNotes; j++)
                                loadedCampaign.addNote(ReadInNote(sr));

                            loadedCalendar.AddCampaign(loadedCampaign);
                        }
                    }
                }
                return loadedCalendar;
            }
            else return ReadInFile(sr);
        }

        public static Note ReadInNote(System.IO.StreamReader sr)
        {
            string noteDate = sr.ReadLine();
            int numImportance = Int32.Parse(sr.ReadLine());
            string content = sr.ReadLine();
            AlertScope importance;
            switch (numImportance)
            {
                case 2:
                    importance = AlertScope.global;
                    break;
                case 1:
                    importance = AlertScope.campaign;
                    break;
                case 0:
                    importance = AlertScope.dontAlert;
                    break;
                default:
                    importance = AlertScope.dontAlert;
                    break;
            }

            return new Note(noteDate, importance, content);
        }
        #endregion
    }
}
