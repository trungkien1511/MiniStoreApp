using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.ViewModels.User
{
    public class CartItemViewModel : INotifyPropertyChanged
    {
        public int CartDetailID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }

        private int quantity;
        public int Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public decimal Total => UnitPrice * Quantity;

        public string ImagePath { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
