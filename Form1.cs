using System.Reflection;
using WinForm_ControlsAndGraphics.Domain;

namespace WinForm_ControlsAndGraphics
{
    public partial class Form1 : Form
    {
        List<Good> goods = new List<Good>();
        public Form1()
        {
            InitializeComponent();
            NUD_Amount.BackColor = Color.DarkGray;
        }

        private void NameChanged(object sender, EventArgs e)
        {
            if (tb_GoodName.Text.Length > 0)
            {
                NUD_Amount.Enabled = true;
                NUD_Amount.BackColor = Color.White;
            }
            else
            {
                NUD_Amount.Value = 0;
                NUD_Amount.Enabled = false;
                NUD_Amount.BackColor = Color.DarkGray;
            }
        }

        private void AmountChanged(object sender, EventArgs e)
        {
            if (NUD_Amount.Value > 0)
            {
                tb_PricePerOne.Enabled = true;
                tb_PriceAll.Enabled = true;
            }
            else
            {
                tb_PricePerOne.Enabled = false;
                tb_PriceAll.Enabled = false;
            }
        }

        private void AddGood_Click(object sender, EventArgs e)
        {

            try
            {
                if (tb_GoodName.Text.Length == 0)
                {
                    throw new Exception("Name is empty");
                }
                if (NUD_Amount.Value == 0)
                {
                    throw new Exception("Amount is 0");
                }
                if (tb_PricePerOne.Text.Length == 0)
                {
                    throw new Exception("Price is empty");
                }
                if (tb_Desc.Text.Length == 0)
                {
                    throw new Exception("Description is empty");
                }
                if (dateTimePicker.Value > DateTime.Now)
                {
                    throw new Exception("Date is in future");
                }
                var good = new Good(tb_GoodName.Text, tb_Desc.Text, (int)NUD_Amount.Value, decimal.Parse(tb_PricePerOne.Text), dateTimePicker.Value);
                goods.Add(good);
                listOfGoods.Items.Clear();
                listOfGoods.Items.AddRange(goods.ToArray());
                listOfGoods.Update();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PricePerOneChanged(object sender, EventArgs e)
        {
            tb_PriceAll.Text = (NUD_Amount.Value * decimal.Parse(tb_PricePerOne.Text)).ToString();
        }

        private void RomoveGoodFromList(object sender, EventArgs e)
        {
            Good good = (Good)listOfGoods.SelectedItem;
            goods.Remove(good);
            listOfGoods.Items.Clear();
            listOfGoods.Items.AddRange(goods.ToArray());
            listOfGoods.Update();
        }

        private void EditGoodItem(object sender, EventArgs e)
        {
            int index = listOfGoods.SelectedIndex;
            Good good = (Good)listOfGoods.SelectedItem;
            good.Name = tb_GoodName.Text;
            good.Description = tb_Desc.Text;
            good.Amount = (int)NUD_Amount.Value;
            good.PricePerOne = decimal.Parse(tb_PricePerOne.Text);
            good.TotalPrice = good.Amount * good.PricePerOne;
            good.Date = dateTimePicker.Value;
            goods[index] = good;
            listOfGoods.Items.Clear();
            listOfGoods.Items.AddRange(goods.ToArray());
            listOfGoods.Update();
        }

        private void SelectedValue(object sender, EventArgs e)
        {
            Good good = (Good)listOfGoods.SelectedItem;
            //MessageBox.Show(good.ToString());
            tb_GoodName.Text = good.Name;
            tb_Desc.Text = good.Description;
            NUD_Amount.Value = good.Amount;
            tb_PricePerOne.Text = good.PricePerOne.ToString();
            tb_PriceAll.Text = good.TotalPrice.ToString();
            dateTimePicker.Value = good.Date;
        }
        private void DrawCord(PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.White, 2), new Point(100, 25), new Point(100, 500));
            e.Graphics.DrawLine(new Pen(Color.White, 2), new Point(75, 475), new Point(875, 475));
            e.Graphics.DrawString("text", new Font("Arial", 10), new SolidBrush(Color.White), new Point(75, 475));
        }
        private void DrawDiagram(PaintEventArgs e, Point point, Brush brush, int h)
        {
            e.Graphics.FillRectangle(brush, point.X, point.Y, 50, h);
        }
        public static List<Color> ColorStructToList()
        {
            return typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public)
                                .Select(c => (Color)c.GetValue(null, null))
                                .ToList();
        }
        private int GetStep(double listSize, double value)
        {
            int percent = (int)((value / listSize) * 100);
            int number_from_percent = (int)((450 * percent) / 100);
            int res = 450 - number_from_percent;
            return res;
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            try
            {
                DrawCord(e);
                if (goods.Count == 0)
                {
                    throw new Exception("������ ������. ��������� �������� ��������!");
                }
                List<Color> ColorsList = ColorStructToList();
                Random random = new Random(DateTime.Now.Millisecond);
                List<int> valuesOfNumberOfGoods = new List<int>();
                var destinctGoods = goods.Select(x => new { x.Name }).Distinct().ToList();
                foreach (var item in destinctGoods)
                {   
                    valuesOfNumberOfGoods.Add(goods.FindAll(x => x.Name == item.Name).Count); 
                }

                int x = 110;
                int y = 25;
                int h = 450;
                int size = goods.Count;
                foreach (var item in valuesOfNumberOfGoods)
                {
                    int step = GetStep(size, item);
                    DrawDiagram(e, new Point(x, y + step), new SolidBrush(ColorsList[random.Next(ColorsList.Count - 1)]), h-step);
                    x += 100;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }
    }
}