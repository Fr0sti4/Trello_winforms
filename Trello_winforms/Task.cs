using System;
using System.Drawing;
using System.Windows.Forms;

public class Task
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Color BackgroundColor { get; set; }

    public Task(string title, string description)
    {
        Title = title;
        Description = description;
        BackgroundColor = Color.LightBlue; // Изначальный цвет
    }

    public Panel CreateTaskPanel(ContextMenuStrip taskMenu)
    {
        Panel taskPanel = new Panel
        {
            Width = 140,
            Height = 100,
            BackColor = BackgroundColor,
            BorderStyle = BorderStyle.FixedSingle,
            ContextMenuStrip = taskMenu
        };

        Label taskTitle = new Label
        {
            Text = Title,
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = false,
            Height = 30,
            BackColor = Color.DarkGray,
            ForeColor = Color.White
        };

        Label taskDescription = new Label
        {
            Text = Description,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.TopLeft,
            Padding = new Padding(5)
        };

        taskPanel.Controls.Add(taskDescription);
        taskPanel.Controls.Add(taskTitle);
        return taskPanel;
    }
}
