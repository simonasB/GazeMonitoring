using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GazeMonitoring.Base {
    public class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo {
        protected readonly Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

        public System.Collections.IEnumerable GetErrors(string propertyName) {
            if (Errors.ContainsKey(propertyName))
                return Errors[propertyName];
            else
                return null;
        }

        protected void InvokeErrorsChanged(params string[] properties) {
            foreach (var property in properties) {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(property));
            }
        }

        public bool HasErrors {
            get { return Errors.Count > 0; }
        }

        protected override void SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null) {
            base.SetProperty<T>(ref member, val, propertyName);
            ValidateProperty(propertyName, val);
        }

        private void ValidateProperty<T>(string propertyName, T value) {
            var results = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(this);
            context.MemberName = propertyName;
            Validator.TryValidateProperty(value, context, results);

            if (results.Any()) {

                Errors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            } else {
                Errors.Remove(propertyName);
            }
            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

    }
}
