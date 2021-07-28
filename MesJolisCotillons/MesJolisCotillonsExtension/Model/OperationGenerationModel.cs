using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesJolisCotillonsOperationGeneratorExtension.Model
{
    public class OperationGenerationModel
    {
        public string OperationName { get; set; }

        public string RelativePathFolder { get; set; }

        public bool HasAdapter { get; set; }

        public bool HasValidations { get; set; }

        public bool HasExecutor { get; set; }

        public bool UseCustomResponseBuilder { get; set; }
    }
}
