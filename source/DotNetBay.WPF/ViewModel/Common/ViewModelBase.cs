using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace DotNetBay.WPF.ViewModel.Common
{
    /// <summary>
    /// Base class for all ViewModels. Parts of the source are based on L. Bugnion's MVVMLight
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Extracts the name of a property from an expression.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the property.
        /// </typeparam>
        /// <param name="propertyExpression">
        /// An expression returning the property's name.
        /// </param>
        /// <returns>
        /// The name of the property returned by the expression.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If the expression is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the expression does not represent a property.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "This design is better than the alternative.")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This design is better than the alternative.")]
        protected static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }

            var property = body.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }

            return property.Name;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            var handler = this.PropertyChanging;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected bool Set<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue) where T : class
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            this.RaisePropertyChanging(propertyExpression);
            
            var oldValue = field;
            field = newValue;
            
            this.RaisePropertyChanged(propertyExpression, oldValue, field);
            return true;
        }

        private void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression, object oldValue, object field)
        {
            var propertyName = GetPropertyName(propertyExpression);
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        private void RaisePropertyChanging<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = GetPropertyName(propertyExpression);
            this.OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
        }
    }
}