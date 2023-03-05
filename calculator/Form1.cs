using System.Linq;
using System.Text.RegularExpressions;

namespace calculator

{
    public partial class Form1 : Form
    {

        List<string> list;
        Dictionary<string, int> priority;
        Dictionary<char, int> signValues;
        Dictionary<int, char> asciiValues;
        HashSet<string> oprators;
        HashSet<char> numbers;
        bool canInput;


        public Form1()
        {
            InitializeComponent();
            list = new List<string>();
            priority = new Dictionary<string, int>();
            oprators = new HashSet<string>();
            numbers = new HashSet<char>();
            signValues = new Dictionary<char, int>();
            asciiValues = new Dictionary<int, char>();
            canInput = true;

            signValues.Add('+', 1);
            signValues.Add('-', -1);

            asciiValues.Add(96, '0');
            asciiValues.Add(97, '1');
            asciiValues.Add(98, '2');
            asciiValues.Add(99, '3');
            asciiValues.Add(100, '4');
            asciiValues.Add(101, '5');
            asciiValues.Add(102, '6');
            asciiValues.Add(103, '7');
            asciiValues.Add(104, '8');
            asciiValues.Add(105, '9');
            asciiValues.Add(191, '÷');
            asciiValues.Add(111, '÷');
            asciiValues.Add(106, 'x');
            asciiValues.Add(186, 'x');
            asciiValues.Add(109, '-');
            asciiValues.Add(189, '-');
            asciiValues.Add(107, '+');
            asciiValues.Add(187, '+');
            asciiValues.Add(110, '.');
            asciiValues.Add(190, '.');

            priority.Add("+", 1);
            priority.Add("-", 1);
            priority.Add("*", 2);
            priority.Add("/", 2);
            priority.Add("x", 2);
            priority.Add("÷", 2);

            oprators.Add("+");
            oprators.Add("-");
            oprators.Add("x");
            oprators.Add("÷");

            numbers.Add('.');
            numbers.Add('0');
            numbers.Add('1');
            numbers.Add('2');
            numbers.Add('3');
            numbers.Add('4');
            numbers.Add('5');
            numbers.Add('6');
            numbers.Add('7');
            numbers.Add('8');
            numbers.Add('9');



        }

        private List<string> preparingString(string text)
        {
            List<string> list = new List<string>();

            char[] chars = text.ToCharArray();
            int length = text.Length;
            int sign = 1;
            char next = '\0';
            char previous = '\0';
            char current;
            
            for (int i = 0; i < length; i++)
            {

                current = chars[i];

                if (i != length - 1)
                    next = chars[i + 1];

                if (i != 0)
                    previous = chars[i - 1];



                //if current and next are + or - and previous not a number
                if ((!numbers.Contains(previous)) && (oprators.Contains(current.ToString())) && (next == '+' || next == '-'))
                {

                    if (current == 'x' || current == '÷')
                    {
                        list.Add(current.ToString());
                    }
                    else
                    {
                        sign *= signValues[chars[i]] * signValues[chars[i + 1]];
                        i = i + 1;
                    }


                }
                //if current is + or - and next is a number
                else if (oprators.Contains(current.ToString()) && numbers.Contains(next))
                {
                    if (oprators.Contains(previous.ToString()) || previous == '\0')
                    {
                        sign *= signValues[chars[i]];
                    }
                    else
                    {
                        list.Add(current.ToString());
                    }


                }
                //if current is + or - and previous is a number
                else if (oprators.Contains(current.ToString()) && numbers.Contains(previous))
                {
                    list.Add(chars[i].ToString());
                }
                //if current is a number
                else
                {
                    List<char> charsList = new List<char>();
                    int j = i;
                    while (j < length && numbers.Contains(chars[j]))
                    {
                        charsList.Add(chars[j]);
                        j++;

                    }
                    i = j - 1;

                    string num = new string(charsList.ToArray());
                    float number = sign * float.Parse(num);
                    sign = 1;
                    list.Add(number.ToString());


                }


            }


            return list;

        }

        private float calculate(string op, float firstOprant, float secondOprant)
        {

            switch (op)
            {
                case "+":
                    return firstOprant + secondOprant;

                case "-":
                    return firstOprant - secondOprant;

                case "x":
                    return firstOprant * secondOprant;

                case "÷":
                    return firstOprant / secondOprant;

                default: return 0;

            }

        }

        private List<string> InfixToPostfix(List<string> inputs)
        {
            List<string> postfixList = new List<string>();

            Stack<string> stack = new Stack<string>();

            foreach (var item in inputs)
            {
                // if a number

                if (!oprators.Contains(item) && item != "​​​​​")
                {
                    postfixList.Add(item);
                }
                // if is not a number and stack not empty and priority high than top of stack
                else if (oprators.Contains(item) && stack.Count != 0 && priority[item] > priority[stack.Peek()])
                {
                    stack.Push(item);
                }
                // if is not a number and stack empty 
                else if (oprators.Contains(item) && stack.Count == 0)
                {
                    stack.Push(item);
                }
                else
                {
                    // if is not a number and stack not empty and priority less than top of stack
                    while (oprators.Contains(item) && stack.Count != 0 && priority[item] <= priority[stack.Peek()])
                    {
                        postfixList.Add(stack.Pop());

                    }

                    stack.Push(item);

                }

            }//end for

            // if stack not empty 
            while (stack.Count != 0)
            {
                postfixList.Add(stack.Pop());
            }

            // postfixList.ForEach(item => MessageBox.Show(item));


            return postfixList;

        }

        private float PostfixEvaluation(List<string> inputs)
        {
            Stack<float> stack = new Stack<float>();
            float firstOprant, secondOprant;

            foreach (var item in inputs)
            {

                if (!oprators.Contains(item))
                {
                    string s = item;
                    s = Regex.Replace(s, @"[^\u0000-\u007F]+", string.Empty);
                    float f = float.Parse(s);
                    stack.Push(f);
                }
                else
                {
                    secondOprant = stack.Pop();
                    firstOprant = stack.Pop();
                    stack.Push(calculate(item, firstOprant, secondOprant));
                }
            }

            if (stack.Count == 1)
            {
                return stack.Pop();
            }



            return 0;

        }

        // buttons
        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void point_Click(object sender, EventArgs e)
        {

            //MessageBox.Show();
            if (!result.Text.EndsWith("."))
            {
                result.Text += ".";
            }

        }

        private void zero_Click(object sender, EventArgs e)
        {
            if (canInput)
                result.Text += '0';
        }

        private void one_Click(object sender, EventArgs e)
        {
            if (canInput)
                result.Text += '1';
        }

        private void tow_Click(object sender, EventArgs e)
        {
            if (canInput)
                result.Text += '2';
        }

        private void three_Click(object sender, EventArgs e)
        {
            if (canInput)
                result.Text += '3';
        }

        private void four_Click(object sender, EventArgs e)
        {
            if (canInput)
                result.Text += '4';
        }

        private void five_Click(object sender, EventArgs e)
        {
            if (canInput)
                result.Text += '5';
        }

        private void six_Click(object sender, EventArgs e)
        {
            if (canInput)
                result.Text += '6';
        }

        private void seven_Click(object sender, EventArgs e)
        {
            if (canInput)
                result.Text += '7';
        }

        private void eight_Click(object sender, EventArgs e)
        {
            if (canInput)
                result.Text += '8';
        }

        private void nine_Click(object sender, EventArgs e)
        {
            if (canInput)
                result.Text += '9';
        }

        private void sum_Click(object sender, EventArgs e)
        {
            if (canInput)
                result.Text += '+';
        }

        private void sub_Click(object sender, EventArgs e)
        {
            if (canInput)
                result.Text += '-';
        }

        private void multiply_Click(object sender, EventArgs e)
        {
            if (canInput)
                result.Text += 'x';
        }

        private void division_Click(object sender, EventArgs e)
        {
            if (canInput)
                result.Text += '÷';
        }

        private void equalButton_Click(object sender, EventArgs e)
        {

            if (canInput)
            {


                try
                {
                    List<string> postfixList = InfixToPostfix(preparingString(result.Text));
                    result.Text = PostfixEvaluation(postfixList).ToString();

                }
                catch (Exception ex)
                {
                    canInput = false;
                    result.Text = "Error 😡";
                }
            }


        }

        private void del_Click(object sender, EventArgs e)
        {
            if (canInput && result.Text.Length > 0)
                result.Text = result.Text.Remove(result.Text.Length - 1);
        }

        private void ac_Click(object sender, EventArgs e)
        {
            
            result.Text = "";
            canInput = true;
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            

            if (e.KeyValue == 46)
            {
                result.Text = "";
                canInput = true;
            }

            if (canInput)
            {


                if (char.IsDigit((char)e.KeyValue))
                {
                    result.Text += ((char)e.KeyValue);
                }
                else if (asciiValues.ContainsKey(e.KeyValue))
                {
                    result.Text += asciiValues[e.KeyValue];
                }
                 
                else if (e.KeyValue == 8)
                {
                    if (result.Text.Length > 0)
                        result.Text = result.Text.Remove(result.Text.Length - 1);
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    try
                    {
                        List<string> postfixList = InfixToPostfix(preparingString(result.Text));
                        string text = PostfixEvaluation(postfixList).ToString();
                        result.Text = text;

                    }
                    catch (Exception ex)
                    {
                        canInput = false;
                        result.Text = "Error 😡";
                    }
                }






            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
