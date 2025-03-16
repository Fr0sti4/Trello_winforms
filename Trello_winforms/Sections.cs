using System;
using System.Drawing;
using System.Windows.Forms;

public class Sections
{
    private Control parent;  // Батьківський контроль, в якому будуть розміщуватися секції та задачі
    private int sectionWidth = 150;  // Ширина кожної секції
    private int sectionSpacing = 5;  // Проміжок між секціями
    private ContextMenuStrip sectionMenu;  // Меню для секцій
    private ContextMenuStrip taskMenu;  // Меню для задач

    public Sections(Control parentControl)
    {
        parent = parentControl;
        InitializeSectionMenu();  // Ініціалізація контекстного меню для секцій
        InitializeTaskMenu();  // Ініціалізація контекстного меню для задач
    }

    // Ініціалізація меню для секцій
    private void InitializeSectionMenu()
    {
        sectionMenu = new ContextMenuStrip();

        ToolStripMenuItem renameItem = new ToolStripMenuItem("Rename");  // Пункт для перейменування секції
        renameItem.Click += RenameSection;

        ToolStripMenuItem changeColorItem = new ToolStripMenuItem("Change color");  // Пункт для зміни кольору секції
        changeColorItem.Click += ChangeSectionColor;

        ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete section");  // Пункт для видалення секції
        deleteItem.Click += DeleteSection;

        ToolStripMenuItem addTaskItem = new ToolStripMenuItem("Add Task");  // Пункт для додавання задачі
        addTaskItem.Click += AddTaskToSection;

        sectionMenu.Items.Add(renameItem);
        sectionMenu.Items.Add(changeColorItem);
        sectionMenu.Items.Add(deleteItem);
        sectionMenu.Items.Add(addTaskItem);  // Додаємо пункт для додавання задачі в секцію
    }

    // Ініціалізація меню для задач
    private void InitializeTaskMenu()
    {
        taskMenu = new ContextMenuStrip();

        ToolStripMenuItem renameTaskItem = new ToolStripMenuItem("Rename Task");  // Пункт для перейменування задачі
        renameTaskItem.Click += RenameTask;

        ToolStripMenuItem changeDescriptionItem = new ToolStripMenuItem("Change Description");  // Пункт для зміни опису задачі
        changeDescriptionItem.Click += ChangeTaskDescription;

        ToolStripMenuItem changeColorItem = new ToolStripMenuItem("Change Color");  // Пункт для зміни кольору задачі
        changeColorItem.Click += ChangeTaskColor;

        ToolStripMenuItem deleteTaskItem = new ToolStripMenuItem("Delete Task");  // Пункт для видалення задачі
        deleteTaskItem.Click += DeleteTask;

        ToolStripMenuItem moveToSectionItem = new ToolStripMenuItem("Move to Section");  // Пункт для переміщення задачі в іншу секцію
        moveToSectionItem.Click += ShowSectionsMenuForTask;

        taskMenu.Items.Add(renameTaskItem);
        taskMenu.Items.Add(changeDescriptionItem);
        taskMenu.Items.Add(changeColorItem);
        taskMenu.Items.Add(deleteTaskItem);
        taskMenu.Items.Add(moveToSectionItem);  // Додаємо пункт для переміщення задачі
    }

    // Відображення меню для вибору секції при переміщенні задачі
    private void ShowSectionsMenuForTask(object sender, EventArgs e)
    {
        RemoveSelectSectionMenuItem();
        // Створюємо підменю для вибору секції
        ToolStripMenuItem moveToSectionSubMenu = new ToolStripMenuItem("Select Section");

        // Перебираємо всі секції та додаємо їх як підменю
        foreach (Control ctrl in parent.Controls)
        {
            if (ctrl is Panel section)
            {
                ToolStripMenuItem sectionItem = new ToolStripMenuItem(section.Controls[0].Text); // Назва секції
                sectionItem.Click += (s, args) => MoveTaskToSection(section);  // При натисканні переміщаємо задачу
                moveToSectionSubMenu.DropDownItems.Add(sectionItem);
            }
        }

        // Додаємо підменю до основного контекстного меню
        taskMenu.Items.Add(moveToSectionSubMenu);
        taskMenu.Show(Cursor.Position);  // Показуємо меню в позиції курсора
    }

    // Переміщення задачі в іншу секцію
    private void MoveTaskToSection(Panel targetSection)
    {
        if (taskMenu.SourceControl is Panel taskPanel)
        {
            // Видаляємо задачу з поточної секції
            Panel currentSection = taskPanel.Parent as Panel;
            currentSection.Controls.Remove(taskPanel);

            // Додаємо задачу в нову секцію
            int newY = 35;
            foreach (Control ctrl in targetSection.Controls)
            {
                if (ctrl is Panel existingTaskPanel)
                {
                    newY = existingTaskPanel.Bottom + 10;  // Позиціонуємо задачу після останньої задачі
                }
            }

            taskPanel.Location = new Point(5, newY);
            targetSection.Controls.Add(taskPanel);  // Додаємо задачу до нової секції

            // Оновлюємо порядок задач в секції
            RepositionTasks(targetSection);

            RemoveSelectSectionMenuItem();
        }
    }

    // Видалення пункту "Select Section" з контекстного меню
    private void RemoveSelectSectionMenuItem()
    {
        foreach (ToolStripItem item in taskMenu.Items)
        {
            if (item is ToolStripMenuItem menuItem && menuItem.Text == "Select Section")
            {
                taskMenu.Items.Remove(menuItem);
                break;
            }
        }
    }

    // Додавання нової секції
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

        // Розміщуємо секцію
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

    // Додавання задачі до секції
    public void AddTaskToSection(object sender, EventArgs e)
    {
        if (sectionMenu.SourceControl is Panel section)
        {
            Task newTask = new Task("New Task", "Description of the task");
            int newY = 35;

            foreach (Control ctrl in section.Controls)
            {
                if (ctrl is Panel existingTaskPanel)
                {
                    newY = existingTaskPanel.Bottom + 10;
                }
            }

            Panel newTaskPanel = newTask.CreateTaskPanel(taskMenu);
            newTaskPanel.Location = new Point(5, newY);
            section.Controls.Add(newTaskPanel);
        }
    }

    // Переміщення задач по секції
    private void RepositionTasks(Panel section)
    {
        int newY = 35;

        foreach (Control ctrl in section.Controls)
        {
            if (ctrl is Panel taskPanel)
            {
                taskPanel.Location = new Point(5, newY);  // Розміщуємо задачу
                newY = taskPanel.Bottom + 10;  // Оновлюємо позицію для наступної задачі
            }
        }
    }

    // Перейменування задачі
    private void RenameTask(object sender, EventArgs e)
    {
        if (taskMenu.SourceControl is Panel taskPanel)
        {
            Label taskTitle = taskPanel.Controls[1] as Label;
            string newTitle = Microsoft.VisualBasic.Interaction.InputBox("Enter new task title:", "Renaming Task", taskTitle.Text);
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                taskTitle.Text = newTitle;
            }
        }
    }

    // Зміна опису задачі
    private void ChangeTaskDescription(object sender, EventArgs e)
    {
        if (taskMenu.SourceControl is Panel taskPanel)
        {
            Label taskDescription = taskPanel.Controls[0] as Label;
            string newDescription = Microsoft.VisualBasic.Interaction.InputBox("Enter new task description:", "Change Task Description", taskDescription.Text);
            if (!string.IsNullOrWhiteSpace(newDescription))
            {
                taskDescription.Text = newDescription;
            }
        }
    }

    // Зміна кольору задачі
    private void ChangeTaskColor(object sender, EventArgs e)
    {
        if (taskMenu.SourceControl is Panel taskPanel)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    taskPanel.BackColor = colorDialog.Color;
                }
            }
        }
    }

    // Видалення задачі
    private void DeleteTask(object sender, EventArgs e)
    {
        if (taskMenu.SourceControl is Panel taskPanel)
        {
            Panel section = taskPanel.Parent as Panel;
            section.Controls.Remove(taskPanel);
            taskPanel.Dispose();

            RepositionTasks(section);  // Після видалення задачі оновлюємо розташування інших
        }
    }

    // Перейменування секції
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

    // Зміна кольору секції
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

    // Видалення секції
    private void DeleteSection(object sender, EventArgs e)
    {
        if (sectionMenu.SourceControl is Panel section)
        {
            parent.Controls.Remove(section);
            section.Dispose();

            RepositionSections();  // Оновлюємо розташування секцій
        }
    }

    // Оновлення розташування секцій після видалення
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
