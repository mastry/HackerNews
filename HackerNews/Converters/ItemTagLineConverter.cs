using HackerNews.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HackerNews.Converters
{
    class ItemTagLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as Item;
            var time = DateTimeOffset.FromUnixTimeSeconds(item.Time);
            var elapsed = GetElapsed(DateTime.Now - time);

            return $"{item.Score} points by {item.By} {elapsed}";
        }

        string GetElapsed(TimeSpan ts)
        {
            if (ts.Days > 0) return $"{ts.Days} days ago";

            if (ts.Hours > 0) return $"{ts.Hours} hours ago";

            if (ts.Minutes > 0) return $"{ts.Minutes} minutes ago";

            return $"{ts.Seconds} seconds ago";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
