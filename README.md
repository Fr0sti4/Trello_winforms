# 📊 **Модель якості для WinForms-додатка (аналог Trello)**

Розробка настільного додатка для управління задачами на WinForms потребує спеціальної моделі якості, яка враховує особливості десктопної архітектури, локального зберігання даних і інтерактивного UI.

## 1. ✨ Функціональна придатність (Functional Suitability)

✅ **Правильність** – додаток повинен коректно виконувати всі CRUD-операції (створення, редагування, видалення, переміщення задач).  
✅ **Повнота** – підтримка кількох списків задач, користувацьких міток, пошуку та фільтрації.  
✅ **Відповідність** – наявність основних функцій Trello, адаптованих для десктопу (drag & drop, контекстне меню).  

### Метрики:
- ▶️ Відсоток реалізованих функцій відносно запланованих.
- ▶️ Середній час виконання операцій (збереження, завантаження, переміщення).

## 2. ⚡ Продуктивність та ефективність (Performance Efficiency)

✅ **Часова ефективність** – швидкість відкриття додатка, завантаження списків та рендерингу UI.  
✅ **Ресурсна ефективність** – мінімальне використання оперативної пам’яті та CPU.  
✅ **Ємність** – ефективне управління великими обсягами даних (1000+ задач).  

### Метрики:
- ▶️ Середній час завантаження списку задач (мс).
- ▶️ Використання оперативної пам’яті (MB).
- ▶️ FPS рендерингу інтерфейсу (>30).

## 3. 🌐 Сумісність (Compatibility)

✅ **Міжплатформність** – коректна робота на Windows 10/11.  
✅ **Співіснування** – інтеграція з локальними файлами (JSON, SQLite), можливість імпорту/експорту даних.  

### Метрики:
- ▶️ Кількість підтримуваних ОС.
- ▶️ Час інтеграції з іншими системами (API, імпорт даних).

## 4. 🛠️ Зручність використання (Usability)

✅ **Простота освоєння** – інтуїтивний інтерфейс, гарячі клавіші, зручне переміщення задач.  
✅ **Оперативність** – мінімальна кількість кліків для основних операцій.  
✅ **Захист від помилок** – підтвердження перед видаленням задач, автозбереження.  

### Метрики:
- ▶️ Час навчання нових користувачів (<10 хв).
- ▶️ Кількість дій для виконання основних операцій.
- ▶️ Відсоток успішних дій без помилок.

## 5. ⚖️ Надійність (Reliability)

✅ **Зрілість** – відсутність критичних помилок.  
✅ **Толерантність до відмов** – система не повинна аварійно завершуватись при введенні некоректних даних.  
✅ **Доступність** – відсутність зависань та вильотів.  

### Метрики:
- ▶️ Кількість збоїв за 1000 операцій.
- ▶️ Час відновлення після збою (автоматичне збереження).

## 6. ⚠️ Безпека (Security)

✅ **Конфіденційність** – локальне зберігання даних із можливістю паролювання.  
✅ **Цілісність** – захист файлів від некоректних змін.  
✅ **Контроль доступу** – розмежування прав доступу (якщо передбачається багатокористувацький режим).  

### Метрики:
- ▶️ Час, необхідний для злому даних (брутфорс, SQL-ін’єкції).
- ▶️ Кількість знайдених вразливостей.

## 7. 🛠️ Обслуговуваність (Maintainability)

✅ **Модульність** – розділення на компоненти (UI, логіка, база даних).  
✅ **Змінюваність** – легкість додавання нових функцій.  
✅ **Тестованість** – покриття коду юніт-тестами.  

### Метрики:
- ▶️ Відсоток покриття коду тестами (>70%).
- ▶️ Час внесення змін у код.
- ▶️ Кількість знайдених проблем у код-рев’ю.

## 8. 💻 Портативність (Portability)

✅ **Адаптивність** – коректна робота на різних розмірах екранів (Full HD, 4K).  
✅ **Переносимість** – легка установка без складних залежностей.  

### Метрики:
- ▶️ Час встановлення додатка.
- ▶️ Відсоток тестів, які проходять на різних версіях Windows.

# 📊 **Специфікаця обмежень в Alloy**
```
sig Task {
    title: one String,
    description: one String,
    backgroundColor: one String,  // Колір фону задачі
    location: one Point
}

sig Section {
    name: one String,
    tasks: set Task  // Секція містить набір задач
}

sig Point {
    x: Int,
    y: Int
}

fact {
    // Обмеження на розміри задачі
    all t: Task | t.location.x >= 0 and t.location.y >= 0
    all t: Task | t.location.x <= 150 and t.location.y <= 100
    
    // Обмеження на позиціонування задач
    all s: Section, t1, t2: Task |
        t1 in s.tasks and t2 in s.tasks and t1 != t2 =>
            (t1.location.x != t2.location.x or t1.location.y != t2.location.y)
}

// Функція для нової позиції задачі
fun newLocation: Point {
    { p: Point | p.x = 5 and p.y = 35 }
}

// Передикат переміщення задачі до нової секції
pred moveTaskToSection(t: Task, from: Section, to: Section) {
    t in from.tasks
    not t in to.tasks
    t.location in newLocation
}

// Виконання сценарію переміщення задачі
run {
    some s: Section, t: Task | moveTaskToSection[t, s, s]
}
```
# 📊 **Діаграми**

## Діаграма класів
![Діаграма класів](Trello_winforms/UML/class.jpg)
```
@startuml

class Task {
    -title: String
    -description: String
    -backgroundColor: Color
    -location: Point
    +Task(title: String, description: String)
    +createTaskPanel(taskMenu: ContextMenuStrip): Panel
}

class Section {
    -name: String
    -tasks: List<Task>
    +Section(name: String)
    +addTask(task: Task): void
    +removeTask(task: Task): void
    +repositionTasks(): void
}

class Point {
    -x: int
    -y: int
    +Point(x: int, y: int)
}

class ContextMenuStrip {
    +show(): void
}

Task "1" -- "0..*" Section : contains
Section "1" -- "0..*" Task : has

@enduml
```

## Діаграма використання
![Діаграма використання](Trello_winforms/UML/usecase.jpg)
```
@startuml

actor User

rectangle "Task Management System" {
    User --> (Add Task)
    User --> (Rename Task)
    User --> (Change Task Description)
    User --> (Change Task Color)
    User --> (Delete Task)
    User --> (Move Task to Section)
    User --> (Add Section)
    User --> (Rename Section)
    User --> (Change Section Color)
    User --> (Delete Section)
}

@enduml
```
## Діаграма послідовності
![Діаграма послідовності](Trello_winforms/UML/sequence.jpg)
```
@startuml
actor User
participant "Task Management System" as System
participant "Task" as Task
participant "Section" as Section
participant "ContextMenu" as Menu

User -> System: Add Task
System -> Task: Create Task
Task -> System: Return Task Panel
System -> Section: Add Task to Section
Section -> System: Update Section Layout
System -> User: Return Task Panel

User -> System: Move Task to Section
System -> Task: Remove Task from Current Section
System -> Section: Add Task to Target Section
Section -> System: Update Section Layout
System -> User: Return Updated Layout

User -> System: Rename Task
System -> Task: Change Title
Task -> User: Prompt for New Title
User -> Task: Provide New Title
Task -> System: Update Task Title
System -> User: Return Updated Task

User -> System: Rename Section
System -> Section: Change Section Title
Section -> User: Prompt for New Section Title
User -> Section: Provide New Title
Section -> System: Update Section Title
System -> User: Return Updated Section

User -> System: Delete Task
System -> Task: Remove Task from Section
System -> Section: Reposition Tasks
System -> User: Return Updated Layout

User -> System: Delete Section
System -> Section: Remove Section
System -> User: Return Updated Layout

@enduml
```
