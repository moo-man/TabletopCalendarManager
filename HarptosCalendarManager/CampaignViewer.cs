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
        Calendar currentCalendar;
        bool[] expansionMap;

        public CampaignViewer(Calendar cal)
        {
            InitializeComponent();
            currentCalendar = cal;
        }

        public void UpdateCampaigns()
        {
            mapExpansion();
            campaignTree.Nodes.Clear();
            if (currentCalendar.numOfCampaigns() == 0)
            {
                campaignTree.Nodes.Add(new TreeNode("No Campaigns"));
            }

            int campaignCount = 0;
            foreach (Campaign c in currentCalendar.CampaignList)
            {
                campaignTree.Nodes.Add(new TreeNode(c.Name + " (" + c.Tag + ")" ));
                int notesCount = 0;
                foreach (Note n in c.notes)
                {
                    string noteDate = n.Date.ToString();
                  
                    // Find existing date node checks if there is already a note with that same date, so it can be put in that node
                    TreeNode existingNode = FindExistingDateNode(campaignTree.Nodes[campaignCount].Nodes, noteDate);

                    if (existingNode != null)
                    {
                        existingNode.Nodes.Add(new TreeNode(n.NoteContent));
                    }
                    else
                    {
                        campaignTree.Nodes[campaignCount].Nodes.Add(new TreeNode(HarptosCalendar.returnGivenDate(n.Date))); // ADD DATE OF NOTE
                        campaignTree.Nodes[campaignCount].Nodes[notesCount].Nodes.Add(new TreeNode(n.NoteContent));         // ADD NOTE CONTENT UNDER IT
                        notesCount++;
                    }
                    
                }
                campaignCount++;
            }
            ExpandNodesWithMap();
        }

        public void ExpandNodesWithMap()
        {
            int expansionIndex = 0;
            for (int campaignCount = 0; campaignCount < campaignTree.Nodes.Count && expansionIndex < expansionMap.Length; campaignCount++) // Campaign Loop
            {
                if (expansionMap[expansionIndex++] == true) // If campaign is expanded, go into date loop
                {
                    campaignTree.Nodes[campaignCount].Expand();
                    for (int dateCount = 0; dateCount < campaignTree.Nodes[campaignCount].Nodes.Count; dateCount++)
                    {
                        if (expansionMap[expansionIndex++] == true)
                            campaignTree.Nodes[campaignCount].Nodes[dateCount].Expand();
                    }
                }
            }
            campaignTree.Nodes[0].EnsureVisible();
        }


        // Every time the TreeView is updated, it collapses all the nodes, to avoid this, we map out which ones
        // are expanded by using a bool array, true - node is expanded, false - collapsed. 
        // Note: This does not look at EVERY node, if a node is collapsed, the children are not looked at
        // Look at ExpandNodesWithMap() function for details on how the array is read
        public void mapExpansion()
        {
            expansionMap = new bool[campaignTree.GetNodeCount(true)]; // max length of array is the number of nodes
            //for (int i = 0; i < expansionMap.Length; i++)
              //  expansionMap[i] = false;

            int expansionIndex = 0;
            for (int campaignNodeCount = 0; campaignNodeCount < campaignTree.Nodes.Count; campaignNodeCount++)// Campaign Loop
            {
                // This if will happen if a campaign was deleted. When a campaign is deleted, it isn't deleted from view until the tree reupdates
                // So a deleted campaign will still me mapped into mapExpansion, unwanted. This if checks if the campaign still exists before mapping it
                if (currentCalendar.CampaignList.Find(x => x.Name == parseCampaignName(campaignTree.Nodes[campaignNodeCount].Text)) != null)
                {
                    if (campaignTree.Nodes[campaignNodeCount].IsExpanded)
                    {
                        expansionMap[expansionIndex++] = true;

                        for (int dateNodeCount = 0; dateNodeCount < campaignTree.Nodes[campaignNodeCount].Nodes.Count; dateNodeCount++) // Date loop
                        {
                            if (campaignTree.Nodes[campaignNodeCount].Nodes[dateNodeCount].IsExpanded)
                                expansionMap[expansionIndex++] = true;
                            else
                                expansionMap[expansionIndex++] = false;
                        }
                    }
                    else
                        expansionMap[expansionIndex++] = false;
                }
            }
            bool [] shorterMap = new bool[expansionIndex];
            for (int i = 0; i < shorterMap.Length; i++)
            {
                shorterMap[i] = expansionMap[i];
            }

            expansionMap = shorterMap;
        }

        public TreeNode FindExistingDateNode(TreeNodeCollection nodes, string dateString)
        {
            foreach (TreeNode n in nodes)
            {
                if (n.Text == HarptosCalendar.returnGivenDate(dateString))
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
            UpdateCampaigns();
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
            UpdateCampaigns();
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
            for (int i = nameAndTag.Length-1; i >= 0; i--)
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
            //UpdateCampaigns();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (returnSelectedCampaign() == null)
                return;
            EditCampaignDialog editMenu = new EditCampaignDialog(returnSelectedCampaign());
            editMenu.ShowDialog(this);
            currentCalendar.goToCurrentDate();
            UpdateCampaigns();
        }

        private void endButton_Click(object sender, EventArgs e)
        {
            Campaign campaignToEnd = returnSelectedCampaign();
            if (campaignToEnd == null)
                return;
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
            UpdateCampaigns();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (returnSelectedCampaign() == null)
                return;
            else
            {
                Campaign toDelete = returnSelectedCampaign();
                DialogResult result;
                result = MessageBox.Show("Are you sure you want to delete " + toDelete.Name + "?", "Delete Campaign", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                    currentCalendar.CampaignList.Remove(toDelete);
            }
            UpdateCampaigns();
        }

        private void CampaignViewer_Enter(object sender, EventArgs e)
        {
           // UpdateCampaigns();
        }

        private void CampaignViewer_Activated(object sender, EventArgs e)
        {
            // UpdateCampaigns();
        }
    }
}
