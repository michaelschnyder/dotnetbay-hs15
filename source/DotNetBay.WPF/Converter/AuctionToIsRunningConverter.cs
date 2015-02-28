using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using DotNetBay.Model;

namespace DotNetBay.WPF.Converter
{
    public class AuctionToIsRunningConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is Auction)
            {
                var auction = value as Auction;

                return auction.StartDateTimeUtc <= DateTime.UtcNow && !auction.IsClosed ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
