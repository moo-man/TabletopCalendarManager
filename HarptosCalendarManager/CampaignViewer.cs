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

        public CampaignViewer(Calendar cal)
        {
            InitializeComponent();
            currentCalendar = cal;
        }

        public void UpdateCampaigns()
        {
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
                  
                    TreeNode existingNode = findExistingDateNode(campaignTree.Nodes[campaignCount].Nodes, noteDate);

                    if (existingNode != null)
                        existingNode.Nodes.Add(new TreeNode(n.NoteContent));
                    else
                    {
                        campaignTree.Nodes[campaignCount].Nodes.Add(new TreeNode(HarptosCalendar.returnGivenDate(n.Date))); // ADD DATE OF NOTE
                        campaignTree.Nodes[campaignCount].Nodes[notesCount].Nodes.Add(new TreeNode(n.NoteContent));         // ADD NOTE CONTENT UNDER IT
                    }
                    notesCount++;
                }
                campaignCount++;
            }
        }

        public TreeNode findExistingDateNode(TreeNodeCollection nodes, string dateString)
        {
            foreach (TreeNode n in nodes)
            {
                if (n.Text == HarptosCalendar.returnGivenDate(dateString))
                    return n;
            }
            return null;
        }
        
        private void addCampaignButton_Click(object sender, EventArgs e)
        {
            NewCampaignDialog newCampaign = new NewCampaignDialog(currentCalendar, campaignTree, this);
            newCampaign.ShowDialog();
        }

        private void campaignTree_Enter(object sender, EventArgs e)
        {
            UpdateCampaigns();

            //campaignTree.Refresh();
        }

        private void CampaignViewer_Load(object sender, EventArgs e)
        {

        }

        private void makeActiveButton_Click(object sender, EventArgs e)
        {
           //string test1 = campaignTree.SelectedNode.Text;
            //string test2 = currentCalendar.CampaignList[0].Name;

            

            Campaign selectedCampaign = (currentCalendar.CampaignList.Find(x => x.Name.Equals(returnSelectedCampaignName(campaignTree.SelectedNode.Text))));
            if (selectedCampaign != null)
            {
                MessageBox.Show("Successufully activated " + selectedCampaign.Name, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                currentCalendar.setActiveCampaign(selectedCampaign);
            }
            else
            {
                MessageBox.Show("Error: Could not activate campaign.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // because the tag is included in the campaign name in the campaign viewer, we must parse out the real name
        public string returnSelectedCampaignName(string nameAndTag)
        {
            for (int i = 0; i < nameAndTag.Length; i++)
            {
                if (nameAndTag.ElementAt(i) == '(')
                    return nameAndTag.Substring(0, i-1);
            }
            return null;
        }
    }
}
