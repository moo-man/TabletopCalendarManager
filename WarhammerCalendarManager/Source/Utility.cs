using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WarhammerCalendarManager
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
                saveFileDialog1.Filter = "WH calendar files (*.whc)|*.whc|All files (*.*)|*.*";
                saveFileDialog1.DefaultExt = "whc";
                saveFileDialog1.InitialDirectory = Application.StartupPath;
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((outStream = saveFileDialog1.OpenFile()) != null)
                        {
                            writer = new System.IO.StreamWriter(outStream);
                            currentFilePath = saveFileDialog1.FileName;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
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
            JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore

            };

            writer.WriteLine(JsonConvert.SerializeObject(calendarToSave, settings));

            #region OLD FILE FORMAT
            /*
            //Write ImperialCalendar data
            calendarToSave.calendar.saveData(writer);

            // write event data
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
                    int noteCount = calendarToSave.CampaignList[campIndex].notes.Count();

                    writer.WriteLine(timerCount);
                    for (int timerIndex = 0; timerIndex < timerCount; timerIndex++)
                    {
                        writer.WriteLine(calendarToSave.CampaignList[campIndex].timers[timerIndex].message);
                        writer.WriteLine(calendarToSave.CampaignList[campIndex].timers[timerIndex].keepTrack);
                        writer.WriteLine(calendarToSave.CampaignList[campIndex].timers[timerIndex].returnDateString());
                    }

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
            openFileDialog1.Filter = "WH Calendar files (*.whc)|*.whc|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.InitialDirectory = Application.StartupPath;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
     
                    string test = sr.ReadToEnd();
                    //loadedCalendar = ReadInFile(sr);
                    loadedCalendar = readInJSON(Newtonsoft.Json.JsonConvert.DeserializeObject(test));

                    currentFilePath = openFileDialog1.FileName;
                    sr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file. Original error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return loadedCalendar;
        }

        public static CalendarContents readInJSON(dynamic json)
        {
            try
            {
                return new CalendarContents(json);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
    }
}
