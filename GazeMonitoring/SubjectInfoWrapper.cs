using System.ComponentModel.DataAnnotations;
using GazeMonitoring.Model;

namespace GazeMonitoring {
    public class SubjectInfoWrapper : ValidatableBindableBase {
        private string _name;
        private int? _age;
        private string _details;

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name {
            get { return _name; }
            set => SetProperty(ref _name, value);
        }

        [Required]
        [Range(2, 150)]
        public int? Age {
            get { return _age; }
            set => SetProperty(ref _age, value);
        }

        [StringLength(500)]
        public string Details {
            get { return _details; }
            set => SetProperty(ref _details, value);
        }

        public void ResetErrors() {
            Errors.Clear();
            InvokeErrorsChanged("Name", "Age", "Details");
        }

        public DataStream DataStream { get; set; }
    }
}
