using System.Drawing;
using System.Windows.Forms;

public class Task
{
    // Властивості класу Task: назва, опис та колір фону
    public string Title { get; set; }
    public string Description { get; set; }
    public Color BackgroundColor { get; set; }

    // Конструктор класу Task, який ініціалізує назву, опис та колір фону
    public Task(string title, string description)
    {
        Title = title;  // Присвоєння назви задачі
        Description = description;  // Присвоєння опису задачі
        BackgroundColor = Color.LightBlue; // Ініціалізація фону за замовчуванням
    }

    // Метод для створення панелі задачі
    public Panel CreateTaskPanel(ContextMenuStrip taskMenu)
    {
        // Створення панелі для задачі
        Panel taskPanel = new Panel
        {
            Width = 140,  // Ширина панелі
            Height = 100,  // Висота панелі
            BackColor = BackgroundColor,  // Колір фону задачі
            BorderStyle = BorderStyle.FixedSingle,  // Оформлення рамки
            ContextMenuStrip = taskMenu  // Призначення контекстного меню для задачі
        };

        // Створення мітки для назви задачі
        Label taskTitle = new Label
        {
            Text = Title,  // Встановлення тексту в мітці (назва задачі)
            Dock = DockStyle.Top,  // Встановлення розташування мітки (вгорі панелі)
            TextAlign = ContentAlignment.MiddleCenter,  // Вирівнювання тексту по центру
            AutoSize = false,  // Вимикаємо автоматичний розмір
            Height = 30,  // Висота мітки
            BackColor = Color.DarkGray,  // Колір фону мітки
            ForeColor = Color.White  // Колір тексту мітки
        };

        // Створення мітки для опису задачі
        Label taskDescription = new Label
        {
            Text = Description,  // Встановлення тексту в мітці (опис задачі)
            Dock = DockStyle.Fill,  // Встановлення розташування (заповнення всієї доступної площі)
            TextAlign = ContentAlignment.TopLeft,  // Вирівнювання тексту вгорі ліворуч
            Padding = new Padding(5)  // Встановлення відступів
        };

        // Додавання мітки опису та назви до панелі задачі
        taskPanel.Controls.Add(taskDescription);
        taskPanel.Controls.Add(taskTitle);

        // Повертаємо створену панель
        return taskPanel;
    }
}
