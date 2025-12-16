using System.Drawing;
using System.Windows.Forms;

namespace praktica
{
    /// <summary>
    /// Простая тема оформления для всех форм приложения.
    /// Делает интерфейс более современным (единые цвета, шрифт, стиль кнопок и таблиц).
    /// </summary>
    public static class UiTheme
    {
        private static readonly Color Background = Color.FromArgb(30, 32, 40);
        private static readonly Color PanelBackground = Color.FromArgb(40, 42, 54);
        private static readonly Color Accent = Color.FromArgb(0, 122, 204);
        private static readonly Color AccentHover = Color.FromArgb(28, 151, 234);
        private static readonly Color TextColor = Color.WhiteSmoke;

        public static void Apply(Form form)
        {
            form.BackColor = Background;
            form.ForeColor = TextColor;
            form.Font = new Font("Segoe UI", 9F);

            StyleControls(form.Controls);
        }

        private static void StyleControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                // Общий фон/цвет текста
                if (!(control is Button))
                {
                    control.BackColor = control is Panel || control is GroupBox
                        ? PanelBackground
                        : Background;
                    control.ForeColor = TextColor;
                }

                switch (control)
                {
                    case Button btn:
                        StyleButton(btn);
                        break;
                    case DataGridView grid:
                        StyleGrid(grid);
                        break;
                    case TextBox tb:
                        tb.BorderStyle = BorderStyle.FixedSingle;
                        tb.BackColor = Color.FromArgb(45, 47, 59);
                        tb.ForeColor = TextColor;
                        break;
                    case ComboBox cb:
                        cb.FlatStyle = FlatStyle.Flat;
                        cb.BackColor = Color.FromArgb(45, 47, 59);
                        cb.ForeColor = TextColor;
                        break;
                    case GroupBox gb:
                        gb.ForeColor = TextColor;
                        break;
                }

                // Рекурсивно стилизуем дочерние элементы
                if (control.HasChildren)
                {
                    StyleControls(control.Controls);
                }
            }
        }

        private static void StyleButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Accent;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) => { btn.BackColor = AccentHover; };
            btn.MouseLeave += (s, e) => { btn.BackColor = Accent; };
        }

        private static void StyleGrid(DataGridView grid)
        {
            grid.EnableHeadersVisualStyles = false;
            grid.BackgroundColor = Background;
            grid.BorderStyle = BorderStyle.None;

            grid.ColumnHeadersDefaultCellStyle.BackColor = PanelBackground;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            grid.DefaultCellStyle.BackColor = Color.FromArgb(45, 47, 59);
            grid.DefaultCellStyle.ForeColor = TextColor;
            grid.DefaultCellStyle.SelectionBackColor = Accent;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;

            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(50, 52, 64);
        }
    }
}


