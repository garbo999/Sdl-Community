﻿using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Sdl.Community.xmlReader.View
{
    public partial class XmlReaderForm : Form
    {
        public XmlReaderForm()
        {
            InitializeComponent();

            labelInstructions.Text = PluginResources.Instruction_Title;
            textBoxInstructions.Text = PluginResources.Instructions_Message;
            labelInstructions.Font = new Font("Arial", 11, FontStyle.Bold);

            labelReports.Text = PluginResources.Reports_Title;
            labelReports.Font = new Font("Arial", 11, FontStyle.Bold);

            buttonConvertToExcel.BackColor = Color.FromArgb(80, 120, 200);

            treeViewXmlFiles.ItemHeight = 20;
        }

        private int FindNodeIndexByLanguageCode(string langCode)
        {
            foreach (TreeNode node in treeViewXmlFiles.Nodes)
            {
                if (langCode.Equals(node.Text.ToString()))
                {
                    return node.Index;
                }
            }

            return 0;
        }

        private int FindImageIndex(string imgCode)
        {
            foreach (var img in treeViewXmlFiles.ImageList.Images.Keys)
            {
                if (imgCode.Equals((img.Split('.'))[0]))
                {
                    return treeViewXmlFiles.ImageList.Images.IndexOfKey(img);
                }
            }

            return 0;
        }

        private string FindTargetLanguageCode(string fileName)
        {
            return ((fileName.Split('_')[1]).ToString()).Substring(0, 5);
        }

        private void treeViewXmlFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] xmlFilePaths = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            int i;
            for (i = 0; i < xmlFilePaths.Length; i++)
            {
                var fileName = Path.GetFileNameWithoutExtension(xmlFilePaths[i]);
                var targetlangCode = FindTargetLanguageCode(fileName);

                var indexNodeByLanguageCode = FindNodeIndexByLanguageCode(targetlangCode);
                if (indexNodeByLanguageCode != 0)
                {
                    treeViewXmlFiles.Nodes[indexNodeByLanguageCode].Nodes.Add("_", fileName, 10000);
                }
                else
                {
                    if (treeViewXmlFiles.Nodes.Count >= 1 && treeViewXmlFiles.Nodes[0].Text.ToString().Equals(targetlangCode))
                        treeViewXmlFiles.Nodes[0].Nodes.Add("_", fileName, 10000);
                    else
                    {
                        treeViewXmlFiles.Nodes.Add(targetlangCode);
                        indexNodeByLanguageCode = FindNodeIndexByLanguageCode(targetlangCode);
                        treeViewXmlFiles.Nodes[indexNodeByLanguageCode].Nodes.Add("_", fileName, 10000);
                    }
                }
            }

            treeViewXmlFiles.ExpandAll();
            treeViewXmlFiles.ShowPlusMinus = false;
            treeViewXmlFiles.ShowRootLines = false;
        }

        private void treeViewXmlFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }
    }
}
