using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HarptosCalendarManager
{
    public partial class CampaignViewer : Form
    {
        CalendarContents currentCalendar;
        List<string> expandedNodes;
        TimeDifference timeDiffTool;
        bool measuring;
        string dateFormat;

        public CampaignViewer(CalendarContents cal)
        {
            InitializeComponent();
            currentCalendar = cal;
            activateToolTip.SetToolTip(makeActiveButton, "Activate a campaign to use it in the Day Tracker");
            campaignTree.ContextMenuStrip = campaignContextMenu;
            timeDiffTool = new TimeDifference();
            measuring = false;
            timeDiffTool.VisibleChanged += CheckIfMeasuring;
            dateFormat = "mmm d, yyyy";
        }

        private void CheckIfMeasuring(object sender, EventArgs e)
        {
            if (timeDiffTool.Visible)
                measuring = true;
            else
                measuring = false;
        }

        public void UpdateTree()
        {
            expandedNodes = ListOfExpandedNodes(campaignTree);
            campaignTree.Nodes.Clear();
            int campaignCount;

            if (currentCalendar.GeneralNoteList.Count > 0)
            {
                campaignTree.Nodes.Add("General Notes");
                int genNoteCount = 0;
                foreach (Note n in currentCalendar.GeneralNoteList)
                    AddNoteToTree(n, 0, ref genNoteCount);
                campaignCount = 1; // start at 1 if there are general notes (general notes are in the first tree node
            }
            else
                campaignCount = 0;

            if (currentCalendar.numOfCampaigns() == 0)
                campaignTree.Nodes.Add(new TreeNode("No Campaigns"));


            foreach (Campaign c in currentCalendar.CampaignList)
            {
                campaignTree.Nodes.Add(new TreeNode(c.Name + " (" + c.Tag + ")"));
                int notesCount = 0;
                foreach (Note n in c.notes)
                    AddNoteToTree(n, campaignCount, ref notesCount);
                campaignCount++;
            }
            ExpandNodes(expandedNodes, campaignTree);
        }

        public void AddNoteToTree(Note noteToAdd, int campNum, ref int noteNum)
        {
            string noteDate = noteToAdd.Date.ToString();

            // Find existing date node checks if there is already a note with that same date, so it can be put in that node
            TreeNode existingNode = FindExistingDateNode(campaignTree.Nodes[campNum].Nodes, noteDate);

            if (existingNode != null)
            {
                existingNode.Nodes.Add(new TreeNode(noteToAdd.NoteContent));
            }
            else
            {
                campaignTree.Nodes[campNum].Nodes.Add(new TreeNode(HarptosCalendar.ToString(noteToAdd.Date, dateFormat)));// ADD DATE OF NOTE
                campaignTree.Nodes[campNum].Nodes[noteNum].Nodes.Add(new TreeNode(noteToAdd.NoteContent));         // ADD NOTE CONTENT UNDER IT
                noteNum++;
            }
        }


        // TODO: If "General Notes" is expanded, and there is a not-expanded general note that has the same date as an expanded, non-general note, the non-general note will expand on update, or vice-versa



        /// <summary>
        /// Takes a list of string and expands all nodes that have text in the list
        /// </summary>
        /// <param name="nodesText"></param>
        /// <param name="tree"></param>
        public void ExpandNodes(List<string> nodesText, TreeView tree)
        {
            foreach (TreeNode campaignNode in tree.Nodes)
            {
                if (nodesText.Contains(campaignNode.Text))
                {
                    campaignNode.Expand();
                    foreach (TreeNode dateNode in campaignNode.Nodes)
                    {
                        if (nodesText.Contains(dateNode.Text))
                            dateNode.Expand();
                    }
                }

            }
        }

        /// <summary>
        /// Finds the text of all nodes that are expanded, returns list of them
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public List<string> ListOfExpandedNodes(TreeView tree)
        {
            List<string> returnList = new List<string>();
            foreach (TreeNode campaignNode in tree.Nodes)
            {
                if (campaignNode.IsExpanded)
                {
                    returnList.Add(campaignNode.Text);
                    foreach (TreeNode dateNode in campaignNode.Nodes)
                    {
                        if (dateNode.IsExpanded)
                        {
                            returnList.Add(dateNode.Text);
                        }
                    }
                }
            }
            return returnList;
        }

        public TreeNode FindExistingDateNode(TreeNodeCollection nodes, string dateString)
        {
            foreach (TreeNode n in nodes)
            {
                if (n.Text == HarptosCalendar.ToString(dateString, dateFormat))
                    return n;
            }
            return null;
        }

        private void AddCampaignButton_Click(object sender, EventArgs e)
        {
            NewCampaignDialog newCampaign = new NewCampaignDialog(currentCalendar, campaignTree, this);
            newCampaign.ShowDialog(this);
        }

        private void CampaignViewer_Load(object sender, EventArgs e)
        {
            UpdateTree();
        }

        private void makeActiveButton_Click(object sender, EventArgs e)
        {
            Campaign selectedCampaign = returnSelectedCampaign();

            if (selectedCampaign != null)
            {
                if (selectedCampaign.isEnded())
                {
                    if (MessageBox.Show("This campaign has ended, activating it will start it again, continue?", "Campaign has ended", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        return;
                    else
                        selectedCampaign.toggleEnded();

                }

                MessageBox.Show("Successufully activated " + selectedCampaign.Name, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                currentCalendar.setActiveCampaign(selectedCampaign);
            }
            else
            {
                MessageBox.Show("Error: Could not activate campaign.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateTree();
        }


        public Campaign returnSelectedCampaign()
        {
            if (campaignTree.SelectedNode == null)
                return null;

            switch (campaignTree.SelectedNode.Level)
            {
                case 0:
                    return currentCalendar.CampaignList.Find(x => x.Name.Equals(parseCampaignName(campaignTree.SelectedNode.Text)));
                case 1:
                    return currentCalendar.CampaignList.Find(x => x.Name.Equals(parseCampaignName(campaignTree.SelectedNode.Parent.Text)));
                case 2:
                    return currentCalendar.CampaignList.Find(x => x.Name.Equals(parseCampaignName(campaignTree.SelectedNode.Parent.Parent.Text)));
                default:
                    return null;
            }
        }

        // because the tag is included in the campaign name in the campaign viewer, we must parse out the real name
        public string parseCampaignName(string nameAndTag)
        {
            // Loop starts at the end of the string, so parentheses in the campaign name (for whatever reason)
            // doesn't affect the parsing
            for (int i = nameAndTag.Length - 1; i >= 0; i--)
            {
                if (nameAndTag.ElementAt(i) == '(')
                    return nameAndTag.Substring(0, i - 1);
            }
            return null;
        }

        private void deactivateButton_Click(object sender, EventArgs e)
        {
            if (currentCalendar.activeCampaign != null)
            {
                MessageBox.Show("Deactivated " + currentCalendar.activeCampaign.Name, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                currentCalendar.activeCampaign = null;
            }
            else
            {
                MessageBox.Show("No active campaign", "Deactivation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //UpdateCampaigns();
        }

        private void endButton_Click(object sender, EventArgs e)
        {
            Campaign campaignToEnd = returnSelectedCampaign();
            if (campaignToEnd == null)
            {
                MessageBox.Show("Select the campaign you wish to end.", "Select Campaign", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (campaignToEnd.isEnded() == true)
            {
                MessageBox.Show("This campaign has already ended. Activating it will start it again.", "End Campaign", MessageBoxButtons.OK);
                return;
            }
            else if (MessageBox.Show(
                "Are you sure you wish to end this campaign?\n\n" +
                "Ending a campaign will turn the \"Current Date\" note to an \"Ended\" note\n" +
                "and this campaign can't be used in the Day Tracker.\n\n" +
                "This can be reversed by activating the campaign again.",
                "End Campaign", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                if (currentCalendar.activeCampaign == campaignToEnd)
                    currentCalendar.activeCampaign = null;

                campaignToEnd.toggleEnded();
            }
            UpdateTree();
        }

        private void CampaignViewer_Enter(object sender, EventArgs e)
        {
            // UpdateCampaigns();
        }

        private void CampaignViewer_Activated(object sender, EventArgs e)
        {
            // UpdateCampaigns();
        }

        private void editCampaignButton_Click(object sender, EventArgs e)
        {
            if (returnSelectedCampaign() == null)
            {
                MessageBox.Show("Select the campaign you wish to edit.", "Select Campaign", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            NewCampaignDialog editMenu = new NewCampaignDialog(currentCalendar, returnSelectedCampaign(), campaignTree, this);
            editMenu.ShowDialog(this);
            currentCalendar.goToCurrentDate();
            UpdateTree();
        }

        private void deleteCampaignButton_Click(object sender, EventArgs e)
        {
            if (returnSelectedCampaign() == null)
            {
                MessageBox.Show("Select the campaign you wish to delete.", "Select Campaign", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                Campaign toDelete = returnSelectedCampaign();
                DialogResult result;
                result = MessageBox.Show("Are you sure you want to delete " + toDelete.Name + "?", "Delete Campaign", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                    currentCalendar.CampaignList.Remove(toDelete);
            }
            UpdateTree();
        }

        private void addNoteButton_Click(object sender, EventArgs e)
        {
            EditNotesDialog newNoteDialog = new EditNotesDialog(currentCalendar);
            newNoteDialog.ShowDialog(this);
            UpdateTree();
        }

        private void editNoteButton_Click(object sender, EventArgs e)
        {
            if (campaignTree.SelectedNode.Level != 2 || campaignTree.SelectedNode == null)
            {
                MessageBox.Show("Please select the note you wish to edit", "Select Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            noteType type;
            if (campaignTree.SelectedNode.Parent.Parent.Text == "General Notes")
                type = noteType.generalNote;
            else
                type = noteType.note;

            Note noteToEdit = currentCalendar.findNote(campaignTree.SelectedNode.Text, type);
            if (CalendarContents.CanEditOrDelete(noteToEdit))
            {
                EditNotesDialog editNoteDialog = new EditNotesDialog(noteToEdit, currentCalendar);
                editNoteDialog.ShowDialog(this);
                UpdateTree();
            }
            else
            {
                MessageBox.Show(this, "This note cannot be edited.", "Cannot edit note", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void deleteNoteButton_Click(object sender, EventArgs e)
        {
            if (campaignTree.SelectedNode.Level != 2 || campaignTree.SelectedNode == null)
            {
                MessageBox.Show("Please select the note you wish to delete", "Delete Note", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            noteType type;
            if (campaignTree.SelectedNode.Parent.Parent.Text == "General Notes")
                type = noteType.generalNote;
            else
                type = noteType.note;

            Note noteToDelete = currentCalendar.findNote(campaignTree.SelectedNode.Text, type);
            if (CalendarContents.CanEditOrDelete(noteToDelete))
            {
                if (MessageBox.Show("Are you sure you want to delete this note?", "Delete Note", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    currentCalendar.deleteNote(noteToDelete);
            }
            else
            {
                MessageBox.Show(this, "This note cannot be deleted.", "Cannot delete note", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UpdateTree();
        }

        private void campaignTree_MouseDown(object sender, MouseEventArgs e)
        {
            campaignTree.SelectedNode = campaignTree.GetNodeAt(e.X, e.Y);
            if (e.Button == MouseButtons.Right)
                DetermineTreeContextMenu(campaignTree.SelectedNode);

            if (measuring && campaignTree.SelectedNode != null)
            {
                switch (campaignTree.SelectedNode.Level)
                {
                    case 0: // Take the name of the selected node (which is the campaign name), find the current date of that campaign
                        if (campaignTree.SelectedNode.Text != "General Notes" && campaignTree.SelectedNode.Text != "No Campaigns")
                            timeDiffTool.GiveDate(currentCalendar.CampaignList.Find(x => x.Name.Equals(parseCampaignName(campaignTree.SelectedNode.Text))).CurrentDate);
                        break;
                    case 1: //
                        timeDiffTool.GiveDate(HarptosCalendar.ReturnGivenDateFromName(campaignTree.SelectedNode.Text));
                        break;
                    case 2:
                        timeDiffTool.GiveDate(
                            currentCalendar.CampaignList.Find(
                                x => x.Name.Equals(
                                    parseCampaignName( // take the node's parent's parent, which is the campaign name. Find the note in that campaign, give date to timedifftool
                                        campaignTree.SelectedNode.Parent.Parent.Text))).findNote(campaignTree.SelectedNode.Text).Date);
                        break;
                }

            }
        }

        private void DetermineTreeContextMenu(TreeNode selectedNode)
        {
            // edit/delete note buttons
            if (selectedNode == null || campaignTree.SelectedNode.Level != 2)
            {
                (campaignContextMenu.Items[1] as ToolStripMenuItem).DropDownItems[1].Enabled = false;
                (campaignContextMenu.Items[1] as ToolStripMenuItem).DropDownItems[2].Enabled = false;
            }
            else
            {
                (campaignContextMenu.Items[1] as ToolStripMenuItem).DropDownItems[1].Enabled = true;
                (campaignContextMenu.Items[1] as ToolStripMenuItem).DropDownItems[2].Enabled = true;
            }

            // Collapsing/Expanding and edit/delete/end campaign buttons
            if (selectedNode == null)
            {
                (campaignContextMenu.Items[3] as ToolStripMenuItem).Text = "Expand All";
                (campaignContextMenu.Items[4] as ToolStripMenuItem).Text = "Collapse All";

                (campaignContextMenu.Items[0] as ToolStripMenuItem).DropDownItems[1].Enabled = false;
                (campaignContextMenu.Items[0] as ToolStripMenuItem).DropDownItems[2].Enabled = false;
                (campaignContextMenu.Items[0] as ToolStripMenuItem).DropDownItems[3].Enabled = false;

            }
            else
            {
                (campaignContextMenu.Items[3] as ToolStripMenuItem).Text = "Expand Children";
                (campaignContextMenu.Items[4] as ToolStripMenuItem).Text = "Collapse Children";

                (campaignContextMenu.Items[0] as ToolStripMenuItem).DropDownItems[1].Enabled = true;
                (campaignContextMenu.Items[0] as ToolStripMenuItem).DropDownItems[2].Enabled = true;
                (campaignContextMenu.Items[0] as ToolStripMenuItem).DropDownItems[3].Enabled = true;
            }

            // activate/deactivate
            if (currentCalendar.activeCampaign != null)
                (campaignContextMenu.Items[2] as ToolStripMenuItem).Text = "Deactivate";
            else
                (campaignContextMenu.Items[2] as ToolStripMenuItem).Text = "Activate";
        }

        private void activateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCalendar.activeCampaign == null)
                this.makeActiveButton_Click(sender, e);
            else
                this.deactivateButton_Click(sender, e);
        }

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (campaignTree.SelectedNode != null)
            {
                campaignTree.SelectedNode.ExpandAll();
            }
            else
                campaignTree.ExpandAll();
        }

        private void collapseALlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (campaignTree.SelectedNode != null)
            {
                campaignTree.SelectedNode.Collapse();
            }
            else
                campaignTree.CollapseAll();
        }

        private void timeDiffButton_Click(object sender, EventArgs e)
        {
            timeDiffTool.Show();
        }
    }
}