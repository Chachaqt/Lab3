using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EndayaLab
{
    public partial class frmAddProduct : Form
    {
        private BindingSource showProductList;

        public string Product_Name(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new StringFormatException("Product name cannot be empty.");

            if (!Regex.IsMatch(name, @"^[a-zA-Z\s]+$")) 
                throw new StringFormatException("Product name must contain only letters and spaces.");

            return name;
        }

        public int Quantity(string qty)
        {
            if (string.IsNullOrWhiteSpace(qty))
                throw new NumberFormatException("Quantity cannot be empty.");

            if (!Regex.IsMatch(qty, @"^\d+$")) 
                throw new NumberFormatException("Quantity must contain only numbers.");

            return Convert.ToInt32(qty);
        }

        public double SellingPrice(string price)
        {
            if (string.IsNullOrWhiteSpace(price))
                throw new CurrencyFormatException("Price cannot be empty.");

            if (!Regex.IsMatch(price, @"^(\d*\.)?\d+$")) 
                throw new CurrencyFormatException("Price must be a valid currency format.");

            return Convert.ToDouble(price);
        }

        private string _ProductName;
        private string _Category;
        private string _MfgDate;
        private string _ExpDate;
        private string _Description;
        private int _Quantity;
        private double _SellPrice;

        public frmAddProduct()
        {
            InitializeComponent();
            showProductList = new BindingSource();

            string[] ListOfProductCategory = new string[]
            {
                     "Beverages", 
                     "Bread/Bakery", 
                     "Canned/Jarred Goods", 
                     "Dairy",
                     "Frozen Goods", 
                     "Meat", 
                     "Personal Care",
                     "Other"
            };

            foreach (string category in ListOfProductCategory)
            {
                cbCategory.Items.Add(category);
            }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                _ProductName = Product_Name(txtProductName.Text);
                _Category = cbCategory.Text;
                _MfgDate = dtPickerMfgDate.Value.ToString("yyyy-MM-dd");
                _ExpDate = dtPickerExpDate.Value.ToString("yyyy-MM-dd");
                _Description = richTxtDescription.Text;

                try
                {
                    _Quantity = Quantity(txtQuantity.Text);
                }
                catch (NumberFormatException ex)
                {
                    MessageBox.Show($"Quantity Error: {ex.Message}");
                    return;
                }
                finally
                {
                    txtQuantity.Clear();
                }

                try
                {
                    _SellPrice = SellingPrice(txtSellPrice.Text);
                }
                catch (CurrencyFormatException ex)
                {
                    MessageBox.Show($"Price Error: {ex.Message}");
                    return;
                }
                finally
                {
                    txtSellPrice.Clear();
                }

                showProductList.Add(new ProductClass(_ProductName, _Category, _MfgDate,
                    _ExpDate, _SellPrice, _Quantity, _Description));
                gridViewProductList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                gridViewProductList.DataSource = showProductList;
            }
            catch (StringFormatException ex)
            {
                MessageBox.Show($"Product Name Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
            }
            finally
            {
                txtProductName.Clear();
                MessageBox.Show("Product Added.");
            }
        }
    }

    public class StringFormatException : Exception
    {
        public StringFormatException(string message) : base(message) { }
    }

    public class NumberFormatException : Exception
    {
        public NumberFormatException(string message) : base(message) { }
    }

    public class CurrencyFormatException : Exception
    {
        public CurrencyFormatException(string message) : base(message) { }
    }
}