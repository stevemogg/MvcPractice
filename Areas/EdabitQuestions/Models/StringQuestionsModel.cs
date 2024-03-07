using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;

namespace MvcPractice.Areas.EdabitQuestions.Models
{
    public class StringQuestionsModel
    {
        public byte QuestionId { get; set; }
        public StringQuestion SelectedQuestion { get; set; }
        public List<string> p { get; set; }

        //Reflection function to get all the classes here
        //Classes have been named starting with 'NumberQuestion' to easily identify them for this page
        public static List<StringQuestion> GetQuestions()
        {
            Assembly? assembly = Assembly.GetExecutingAssembly();
            string ns = "MvcPractice.Areas.EdabitQuestions.Models";
            Type[]? types = assembly.GetTypes().Where(x => String.Equals(x.Namespace, ns, StringComparison.Ordinal)).ToArray();

            List<StringQuestion> questions = new List<StringQuestion>();
            int typeCount = 0;
            foreach (var type in types)
            {
                if (type.Name.Contains("StringQuestion") && !type.Name.Contains("StringQuestionsModel") && type.Name != "StringQuestion" && type.Name != "NumberQuestion")
                {
                    questions.Add((StringQuestion)Activator.CreateInstance(type, new object[] { typeCount, type.Name.Replace("StringQuestion", ""), null }));
                    typeCount++;
                }
            }
            return questions;
        }
        public List<StringQuestion> QuestionList = GetQuestions();
    }
    //Base class for each question
    public abstract class StringQuestion
    {
        public List<string> ParamNames { get; set; }
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public List<string>? p { get; set; }  // Parameters used to store user input
        public StringQuestion(int questionId, string questionName, List<String> paramNames)
        {
            QuestionId = questionId;
            QuestionName = questionName;
            ParamNames = paramNames;
        }
        public abstract dynamic PerformQuestionLogic();
    }

    public class StringQuestionReturnSomething : StringQuestion
    {
        public StringQuestionReturnSomething(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "X" }) { }
        public override dynamic PerformQuestionLogic()
        {
            return p[0];
        }     
    }
    public class StringQuestionCompareStringLengths : StringQuestion
    {
        public StringQuestionCompareStringLengths(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "Word1","Word2" }) { }
        public override dynamic PerformQuestionLogic()
        {
            if (p[0] == null || p[1] == null || p[0] == "" || p[1] == "")
            {
                return "You need to enter some text!";
            }
            if (p[0].Length > p[1].Length)
            {
                return p[0].Length - p[1].Length;
            }
            else if (p[1].Length > p[0].Length)
            {
                return p[1].Length - p[0].Length;
            }
            else if (p[0] == p[1])
            {
                return "They are the same length";
            }
            else
            {
                return "Something went wrong!";
            }
        }
    }
    public class StringQuestionMaximumCharOccurence : StringQuestion
    {
        public StringQuestionMaximumCharOccurence(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "Text" }) { }
        public override dynamic PerformQuestionLogic()
        {
            //Doesnt handle when there are multiple maximum chars of the same amount of occurence
            Dictionary<char, int> charCountDictionary = new Dictionary<char, int>();
            foreach (var item in p[0])
            {
                if (charCountDictionary.ContainsKey(item))
                {
                    charCountDictionary[item]++;
                }
                else
                {
                    charCountDictionary.Add(item, 1);
                }
            }
            int largestCharCount = 0;
            char? largestChar = null;
            foreach (var item in charCountDictionary)
            {
                if (item.Value > largestCharCount)
                {
                    largestCharCount++;
                    largestChar = item.Key;
                }
            }
            return largestChar;
        }
    }

    public class StringQuestionPasswordValidation: StringQuestion
    {
        public StringQuestionPasswordValidation(int questionId, string questionName, List<string>? paramNames) : base(questionId, questionName, new List<string> { "Password" }) { }
        public override dynamic PerformQuestionLogic()
        {
            //Password must be:
            // - length between 6 and 24 characters
            // - At least one uppercase
            // - At least one lowercase
            // - At least one digit
            // - MAximum of 2 repeated characters next to each other
            // - contains only the allowed special characters

            string specialChars = "!@#$%^&*()+=_-{}[]:;\"'?<>,.";

            bool containsUpperCase = false;
            bool containsLowerCase = false;
            bool containsDigit = false;

            bool hasRepeatedChar = false;
            byte repeatedCharCount = 0;
            char? repeatedChar = null;

            bool hasNonAllowedChar = false;

            foreach (var item in p[0])
            {
                if (Char.IsUpper(item))
                    containsUpperCase = true;
                if (Char.IsLower(item))
                    containsLowerCase = true;
                if (Char.IsDigit(item))
                    containsDigit = true;

                if (!hasRepeatedChar)
                {
                    if (repeatedChar == item)
                    {
                        repeatedCharCount++;
                        repeatedChar = item;
                    }
                    if (repeatedCharCount > 2)
                        hasRepeatedChar = true;
                    repeatedChar = item;
                }

                if (!Char.IsLetterOrDigit(item) || !specialChars.Contains(item))
                    hasNonAllowedChar = true;
            }

            if (p[0].Length < 6 || p[0].Length > 24)
                return "The password must be between 6 and 24 characters long";
            else if (containsUpperCase == false)
                return "The password doesnt contain a uppercase letter";
            else if (containsLowerCase == false)
                return "The password doesnt contain a lowercase letter";
            else if (containsDigit == false)
                return "The password doesnt contain a digit";
            else if (hasRepeatedChar == true)
                return "The password has repeated characters longer than 2";
            else if (hasNonAllowedChar == true)
                return $"The password has a character that is not allowed. Password must only contain numbers, letter and these special character: {specialChars}";
            else
                return "The password is correct";
        }
    }
}
