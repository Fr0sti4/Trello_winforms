using System;
using System.Drawing;
using System.Windows.Forms;

public class Sections
{
    private Control parent; 
    private int sectionWidth = 150; 
    private int sectionSpacing = 5; 
    private ContextMenuStrip sectionMenu; 

    public Sections(Control parentControl)
    {
        parent = parentControl;
        InitializeSectionMenu();
    }

    private void InitializeSectionMenu()
    {
        sectionMenu = new ContextMenuStrip();

        ToolStripMenuItem renameItem = new ToolStripMenuItem("Rename");
        renameItem.Click += RenameSection;

        ToolStripMenuItem changeColorItem = new ToolStripMenuItem("Change color");
        changeColorItem.Click += ChangeSectionColor;

        ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete section");
        deleteItem.Click += DeleteSection;

        sectionMenu.Items.Add(renameItem);
        sectionMenu.Items.Add(changeColorItem);
        sectionMenu.Items.Add(deleteItem);
    }

    public void AddSection()
    {
        Panel section = new Panel
        {
            Width = sectionWidth,
            Height = parent.ClientSize.Height,
            BackColor = Color.LightGray,
            BorderStyle = BorderStyle.FixedSingle,
            ContextMenuStrip = sectionMenu 
        };

        Label titleLabel = new Label
        {
            Text = "New Section",
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = false,
            Height = 30,
            BackColor = Color.DarkGray,
            ForeColor = Color.White,
            ContextMenuStrip = sectionMenu 
        };

        section.Controls.Add(titleLabel);

        int newX = 10;

        foreach (Control ctrl in parent.Controls)
        {
            if (ctrl is Panel)
            {
                newX = ctrl.Right + sectionSpacing;
            }
        }

        section.Location = new Point(newX, 0);
        section.Tag = titleLabel; 
        parent.Controls.Add(section);
    }

    private void RenameSection(object sender, EventArgs e)
    {
        if (sectionMenu.SourceControl is Panel section)
        {
            Label titleLabel = (Label)section.Tag;
            string newName = Microsoft.VisualBasic.Interaction.InputBox("Enter new section name:", "Renaming", titleLabel.Text);
            if (!string.IsNullOrWhiteSpace(newName))
            {
                titleLabel.Text = newName;
            }
        }
    }

    private void ChangeSectionColor(object sender, EventArgs e)
    {
        if (sectionMenu.SourceControl is Panel section)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    section.BackColor = colorDialog.Color;
                }
            }
        }
    }

    private void DeleteSection(object sender, EventArgs e)
    {
        if (sectionMenu.SourceControl is Panel section)
        {
            parent.Controls.Remove(section);
            section.Dispose();

            RepositionSections();
        }
    }

    private void RepositionSections()
    {
        int newX = 10;

        foreach (Control ctrl in parent.Controls)
        {
            if (ctrl is Panel section)
            {
                section.Location = new Point(newX, 0);
                newX = section.Right + sectionSpacing;
            }
        }
    }

}
