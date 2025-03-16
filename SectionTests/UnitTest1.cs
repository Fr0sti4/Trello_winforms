using Moq;
using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;
using Xunit;
using static System.Collections.Specialized.BitVector32;

namespace MyApp.Tests
{
    public class SectionsTests
    {
        [Fact]
        public void AddSection_ShouldAddNewSectionToParent()
        {
            // Arrange: Создаем мок родительского контрола
            var parentControlMock = new Mock<Control>();
            var section = new Sections(parentControlMock.Object);

            // Act: Добавляем новую секцию
            section.AddSection();

            // Assert: Проверяем, что метод Controls.Add был вызван для добавления панели
            parentControlMock.Verify(p => p.Controls.Add(It.IsAny<Panel>()), Times.Once);
        }

        [Fact]
        public void RenameSection_ShouldChangeSectionName()
        {
            // Arrange: Создаем мок родительского контрола и панель
            var parentControlMock = new Mock<Control>();
            var section = new Sections(parentControlMock.Object);

            var panelMock = new Mock<Panel>();
            var labelMock = new Mock<Label>();
            labelMock.Setup(l => l.Text).Returns("Old Name");
            panelMock.Setup(p => p.Controls.Add(It.IsAny<Label>()));
            panelMock.Setup(p => p.Tag).Returns(labelMock.Object);

            parentControlMock.Object.Controls.Add(panelMock.Object);

            // Act: Переименовываем секцию
            section.RenameSection(this, EventArgs.Empty);

            // Assert: Проверяем, что текст был изменен на "New Name"
            labelMock.VerifySet(l => l.Text = "New Name", Times.Once);
        }

        [Fact]
        public void ChangeSectionColor_ShouldChangeSectionBackgroundColor()
        {
            // Arrange: Создаем мок родительского контрола и панель
            var parentControlMock = new Mock<Control>();
            var section = new Sections(parentControlMock.Object);

            var panelMock = new Mock<Panel>();
            panelMock.Setup(p => p.BackColor).Returns(Color.LightGray);

            parentControlMock.Object.Controls.Add(panelMock.Object);

            // Act: Изменяем цвет фона секции
            section.ChangeSectionColor(this, EventArgs.Empty);

            // Assert: Проверяем, что цвет был изменен
            panelMock.VerifySet(p => p.BackColor = It.Is<Color>(c => c == Color.Red), Times.Once);
        }

        [Fact]
        public void DeleteSection_ShouldRemoveSectionFromParent()
        {
            // Arrange: Создаем мок родительского контрола и панель
            var parentControlMock = new Mock<Control>();
            var section = new Sections(parentControlMock.Object);

            var panelMock = new Mock<Panel>();
            parentControlMock.Object.Controls.Add(panelMock.Object);

            // Act: Удаляем секцию
            section.DeleteSection(this, EventArgs.Empty);

            // Assert: Проверяем, что панель была удалена из родителя
            parentControlMock.Verify(p => p.Controls.Remove(panelMock.Object), Times.Once);
        }
    }
}
