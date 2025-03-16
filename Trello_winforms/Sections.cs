using System;
using System.Drawing;
using System.Windows.Forms;

public class Sections
{
    private Control parent;
    private int sectionWidth = 150;
    private int sectionSpacing = 5;
    private ContextMenuStrip sectionMenu;
    private ContextMenuStrip taskMenu;  // Меню для задач

    public Sections(Control parentControl)
    {
        parent = parentControl;
        InitializeSectionMenu();
        InitializeTaskMenu();  // Инициализация меню для задач
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

        ToolStripMenuItem addTaskItem = new ToolStripMenuItem("Add Task");
        addTaskItem.Click += AddTaskToSection;

        sectionMenu.Items.Add(renameItem);
        sectionMenu.Items.Add(changeColorItem);
        sectionMenu.Items.Add(deleteItem);
        sectionMenu.Items.Add(addTaskItem);  // Добавляем пункт для добавления задачи
    }

    private void InitializeTaskMenu()
    {
        taskMenu = new ContextMenuStrip();

        ToolStripMenuItem renameTaskItem = new ToolStripMenuItem("Rename Task");
        renameTaskItem.Click += RenameTask;

        ToolStripMenuItem changeDescriptionItem = new ToolStripMenuItem("Change Description");
        changeDescriptionItem.Click += ChangeTaskDescription;

        ToolStripMenuItem changeColorItem = new ToolStripMenuItem("Change Color");
        changeColorItem.Click += ChangeTaskColor;

        ToolStripMenuItem deleteTaskItem = new ToolStripMenuItem("Delete Task");
        deleteTaskItem.Click += DeleteTask;

        ToolStripMenuItem moveToSectionItem = new ToolStripMenuItem("Move to Section");
        moveToSectionItem.Click += ShowSectionsMenuForTask;

        taskMenu.Items.Add(renameTaskItem);
        taskMenu.Items.Add(changeDescriptionItem);
        taskMenu.Items.Add(changeColorItem);
        taskMenu.Items.Add(deleteTaskItem);
        taskMenu.Items.Add(moveToSectionItem);  // Добавляем пункт для перемещения задачи
    }

    private void ShowSectionsMenuForTask(object sender, EventArgs e)
    {
        RemoveSelectSectionMenuItem();
        // Создаем подменю для выбора раздела
        ToolStripMenuItem moveToSectionSubMenu = new ToolStripMenuItem("Select Section");

        // Перебираем все разделы и добавляем их как подменю
        foreach (Control ctrl in parent.Controls)
        {
            if (ctrl is Panel section)
            {
                ToolStripMenuItem sectionItem = new ToolStripMenuItem(section.Controls[0].Text); // Используем название раздела
                sectionItem.Click += (s, args) => MoveTaskToSection(section);  // При клике перемещаем задачу
                moveToSectionSubMenu.DropDownItems.Add(sectionItem);
            }
        }

        // Добавляем подменю в контекстное меню
        taskMenu.Items.Add(moveToSectionSubMenu);
        taskMenu.Show(Cursor.Position);  // Показываем меню в позиции курсора
    }

    private void MoveTaskToSection(Panel targetSection)
    {
        if (taskMenu.SourceControl is Panel taskPanel)
        {
            // Снимаем задачу с текущего раздела
            Panel currentSection = taskPanel.Parent as Panel;
            currentSection.Controls.Remove(taskPanel);

            // Добавляем задачу в новый раздел
            int newY = 35;
            foreach (Control ctrl in targetSection.Controls)
            {
                if (ctrl is Panel existingTaskPanel)
                {
                    newY = existingTaskPanel.Bottom + 10;  // Позиционируем задачу после последней задачи
                }
            }

            taskPanel.Location = new Point(5, newY);
            targetSection.Controls.Add(taskPanel);  // Добавляем задачу в новый раздел

            // Обновляем порядок задач в разделе, если необходимо
            RepositionTasks(targetSection);

            RemoveSelectSectionMenuItem();
        }
    }

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
    public void AddTaskToSection(object sender, EventArgs e)
    {
        // Проверяем, на каком разделе был вызван контекстный клик
        if (sectionMenu.SourceControl is Panel section)
        {
            // Создаем новую задачу и добавляем ее в раздел
            Task newTask = new Task("New Task", "Description of the task");
            int newY = 35;

            // Определяем позицию для новой задачи (после всех существующих задач в разделе)
            foreach (Control ctrl in section.Controls)
            {
                if (ctrl is Panel existingTaskPanel)
                {
                    newY = existingTaskPanel.Bottom + 10;  // Позиция ниже последней задачи
                }
            }

            // Создаем панель для новой задачи
            Panel newTaskPanel = newTask.CreateTaskPanel(taskMenu);
            newTaskPanel.Location = new Point(5, newY);

            // Добавляем панель задачи в раздел
            section.Controls.Add(newTaskPanel);
        }
    }

    private void RepositionTasks(Panel section)
    {
        int newY = 35;

        foreach (Control ctrl in section.Controls)
        {
            if (ctrl is Panel taskPanel)
            {
                taskPanel.Location = new Point(5, newY);  // Перемещаем задачу вверх
                newY = taskPanel.Bottom + 10;  // Обновляем позицию для следующей задачи
            }
        }
    }

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

    private void DeleteTask(object sender, EventArgs e)
    {
        if (taskMenu.SourceControl is Panel taskPanel)
        {
            Panel section = taskPanel.Parent as Panel;
            section.Controls.Remove(taskPanel);
            taskPanel.Dispose();

            RepositionTasks(section);  // После удаления задачи сдвигаем все задачи
        }
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
