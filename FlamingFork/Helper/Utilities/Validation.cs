using System.Text.RegularExpressions;

namespace FlamingFork.Helper.Utilities
{
    public static class Validation
    {
        #region EmailValidator

        // Summary:
        // Checks email for null values, matches the regex pattern and returns proper
        // error message.
        public static string EmailValidator(string email)
        {
            string emailError;
            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new(emailPattern);
            if (string.IsNullOrWhiteSpace(email))
            {
                emailError = "Email field cannot be empty!";
            }
            else if (!regex.IsMatch(email))
            {
                emailError = "Please enter a valid Email address";
            }
            else
            {
                emailError = string.Empty;
            }

            return emailError;
        }

        #endregion EmailValidator

        #region PasswordValidator

        // Summary:
        // Checks password for null values, matches the regex pattern and returns proper
        // error message.
        public static string PasswordValidator(string password)
        {
            string passwordError;
            var passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            Regex regex = new(passwordPattern);
            if (string.IsNullOrWhiteSpace(password))
            {
                passwordError = "Password field cannot be empty!";
            }
            else if (!regex.IsMatch(password))
            {
                passwordError = "Please enter a valid password";
            }
            else
            {
                passwordError = string.Empty;
            }

            return passwordError;
        }

        #endregion PasswordValidator

        #region ContactNumberValidator

        // Summary:
        // Checks contact number for null values, matches the regex pattern and returns proper
        // error message.
        public static string ContactNumberValidator(string contactNumber)
        {
            string contactNumberError;
            var contactNumberPattern = @"^98\d{8}$";
            Regex regex = new(contactNumberPattern);
            if (string.IsNullOrWhiteSpace(contactNumber))
            {
                contactNumberError = "Contact field cannot be empty!";
            }
            else if (!regex.IsMatch(contactNumber))
            {
                contactNumberError = "Please enter a valid contactNumber";
            }
            else
            {
                contactNumberError = string.Empty;
            }

            return contactNumberError;
        }

        #endregion ContactNumberValidator

        #region FullNameValidator

        // Summary:
        // Checks name for null values, matches the regex pattern and returns proper
        // error message.
        public static string NameValidator(string name)
        {
            string nameError;
            var namePattern = @"^[A-Za-z]+ [A-Za-z]+$";
            Regex regex = new(namePattern);
            if (string.IsNullOrWhiteSpace(name))
            {
                nameError = "Name field cannot be empty!";
            }
            else if (!regex.IsMatch(name))
            {
                nameError = "Please enter a valid full name";
            }
            else
            {
                nameError = string.Empty;
            }

            return nameError;
        }

        #endregion FullNameValidator

        #region AddressValidator

        // Summary:
        // Checks address for null values, matches the regex pattern and returns proper
        // error message.
        public static string AddressValidator(string address)
        {
            string addressError;
            var addressPattern = @"^(?i)(Kathmandu|Lalitpur),\s*([\w\s-]+),\s*([\w\s]+)(,\s*[\w\s-]+)*$";
            Regex regex = new(addressPattern);
            if (string.IsNullOrWhiteSpace(address))
            {
                addressError = "Address field cannot be empty!";
            }
            else if (!regex.IsMatch(address))
            {
                addressError = "Please follow the format: city,ward-name,street,prominent landmark";
            }
            else
            {
                addressError = string.Empty;
            }

            return addressError;
        }

        #endregion AddressValidator
    }
}
