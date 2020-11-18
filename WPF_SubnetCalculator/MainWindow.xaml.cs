using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_SubnetCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Datatypes
        string inputOctet1 = "";
        string inputOctet2 = "";
        string inputOctet3 = "";
        string inputOctet4 = "";
        string inputCIDR = "";

        string inputSubnet1 = "";
        string inputSubnet2 = "";
        string inputSubnet3 = "";
        string inputSubnet4 = "";

        string inputWildcard1 = "";
        string inputWildcard2 = "";
        string inputWildcard3 = "";
        string inputWildcard4 = "";

        string networkClass = "";

        int _octet1;
        int _octet2;
        int _octet3;
        int _octet4;
        int _cidr;

        int _subnet1 = 0;
        int _subnet2 = 0;
        int _subnet3 = 0;
        int _subnet4 = 0;

        int _wildcard1 = 0;
        int _wildcard2 = 0;
        int _wildcard3 = 0;
        int _wildcard4 = 0;

        int wildcardOctet1 = 0;
        int wildcardOctet2 = 0;
        int wildcardOctet3 = 0;
        int wildcardOctet4 = 0;

        double totalHostsBase = 2;
        double totalHostsOutput;
        double usableHosts;
        double multiplier;

        int success;
        string complete = "Calculation Complete";
        string clear = "";

        int substractFromThisToGetRange = 256;
        int rangeCalculatorTemp = 0;
        int rangeCalculator = 0;

        int octetTemp = 0;
        int lowerIP = 0;
        int higherIP = 0;

        int zero = 0;
        int one = 1;
        int twoFiftyFour = 254;
        int twoFiftyFive = 255;

        string firstUsable = "";
        string lastUsable = "";

        Random random = new Random();

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            ClearCalculation();
            ConvertInput();
        }

        public void ConvertInput()
        {
            inputOctet1 = Octet1InputTxt.Text;
            inputOctet2 = Octet2InputTxt.Text;
            inputOctet3 = Octet3InputTxt.Text;
            inputOctet4 = Octet4InputTxt.Text;
            inputCIDR = CIDRinputTxt.Text;

            inputSubnet1 = Subnet1InputTxt.Text;
            inputSubnet2 = Subnet2InputTxt.Text;
            inputSubnet3 = Subnet3InputTxt.Text;
            inputSubnet4 = Subnet4InputTxt.Text;

            inputWildcard1 = Wildcard1InputTxt.Text;
            inputWildcard2 = Wildcard2InputTxt.Text;
            inputWildcard3 = Wildcard3InputTxt.Text;
            inputWildcard4 = Wildcard4InputTxt.Text;

            CheckIPOctets();
        }

        #region Check if input-to-integer conversion is possible

        #region Check IP Octets
        public void CheckIPOctets()
        {
            success = 0;

            _octet1 = CheckInput(inputOctet1);
            _octet2 = CheckInput(inputOctet2);
            _octet3 = CheckInput(inputOctet3);
            _octet4 = CheckInput(inputOctet4);

            if (success != -1)
            {
                CheckValuesIpOctets();
            }
        }

        public int CheckInput(string tempUserInput)
        {
            int output = 0;
            try
            {
                output = Int32.Parse(tempUserInput);
            }
            catch (FormatException)
            {
                WrongData();
                success = -1;
            }

            return output;
        }

        public void CheckValuesIpOctets()
        {
            if (_octet1 > 255 || _octet1 < 0)
            {
                WrongData();
            }
            else if (_octet2 > 255 || _octet2 < 0)
            {
                WrongData();
            }
            else if (_octet3 > 255 || _octet3 < 0)
            {
                WrongData();
            }
            else if (_octet4 > 255 || _octet4 < 0)
            {
                WrongData();
            }
            else
            {
                CheckboxCidrSubnetOrWildcard();
            }
        }
        #endregion

        public void CheckboxCidrSubnetOrWildcard()
        {
            if (Checkbox1CIDR.IsChecked == true)
            {
                success = 0;

                _cidr = CheckInput(inputCIDR);

                if (success != -1)
                {
                    CheckValuesCIDR();
                }
            }
            else if (Checkbox2Netmask.IsChecked == true)
            {
                success = 0;

                _subnet1 = CheckInput(inputSubnet1);
                _subnet2 = CheckInput(inputSubnet2);
                _subnet3 = CheckInput(inputSubnet3);
                _subnet4 = CheckInput(inputSubnet4);

                if (success != -1)
                {
                    CheckValuesSubnetMask();
                }
            }
            else if (Checkbox3Wildcard.IsChecked == true)
            {
                success = 0;

                _wildcard1 = CheckInput(inputWildcard1);
                _wildcard2 = CheckInput(inputWildcard2);
                _wildcard3 = CheckInput(inputWildcard3);
                _wildcard4 = CheckInput(inputWildcard4);

                if (success != -1)
                {
                    GetSubnetMaskFromWildcardMask();
                }
            }
            else
            {
                MessageTxt.Text = "Please select checkbox";
            }
        }

        #region Check CIDR
        public void CheckValuesCIDR()
        {
            if (_cidr > 31)
            {
                WrongData();
            }
            else if (_cidr < 0)
            {
                WrongData();
            }
            else
            {
                GetSubnetMaskFromCIDR();
            }
        }
        #endregion

        #region Check Subnet Mask
        public void CheckValuesSubnetMask()
        {
            if (_subnet1 > 255 || _subnet1 < 0)
            {
                WrongData();
            }
            if (_subnet2 > 255 || _subnet2 < 0)
            {
                WrongData();
            }
            else if (_subnet3 > 255 || _subnet3 < 0)
            {
                WrongData();
            }
            else if (_subnet4 > 255 || _subnet4 < 0)
            {
                WrongData();
            }
            else
            {
                GetCIDRfromSubnetMask();
            }
        }
        #endregion

        #region Check Wildcard Mask
        public void GetSubnetMaskFromWildcardMask()
        {
            _subnet1 = 255 - _wildcard1;
            _subnet2 = 255 - _wildcard2;
            _subnet3 = 255 - _wildcard3;
            _subnet4 = 255 - _wildcard4;

            GetCIDRfromSubnetMask();
        }
        #endregion

        #endregion

        public void DetermineClass()
        {
            if (_octet1 <= 127)
            {
                networkClass = "A";

                if (_cidr < 8 || _cidr > 15)
                {
                    networkClass += " Classless";
                }

                OutputClassTxt.Text = networkClass;
            }
            else if (_octet1 <= 191)
            {
                networkClass = "B";

                if (_cidr < 16 || _cidr > 23)
                {
                    networkClass += " Classless";
                }

                OutputClassTxt.Text = networkClass;
            }
            else if (_octet1 <= 223)
            {
                networkClass = "C";

                if (_cidr < 24)
                {
                    networkClass += " Classless";
                }

                OutputClassTxt.Text = networkClass;
            }
            else if (_octet1 <= 239)
            {
                networkClass = "D (Multicast)";

                OutputClassTxt.Text = networkClass;
            }
            else if (_octet1 <= 255)
            {
                networkClass = "E (Reserved)";

                OutputClassTxt.Text = networkClass;
            }
        }

        public void GetSubnetMaskFromCIDR()
        {
            switch (_cidr)
            {
                #region CIDR Class A
                case 0:
                    _subnet1 = 0;
                    multiplier = 32;
                    break;
                case 1:
                    _subnet1 = 128;
                    multiplier = 31;
                    break;
                case 2:
                    _subnet1 = 192;
                    multiplier = 30;
                    break;
                case 3:
                    _subnet1 = 224;
                    multiplier = 29;
                    break;
                case 4:
                    _subnet1 = 240;
                    multiplier = 28;
                    break;
                case 5:
                    _subnet1 = 248;
                    multiplier = 27;
                    break;
                case 6:
                    _subnet1 = 252;
                    multiplier = 26;
                    break;
                case 7:
                    _subnet1 = 254;
                    multiplier = 25;
                    break;
                case 8:
                    _subnet2 = 0;
                    multiplier = 24;
                    break;
                case 9:
                    _subnet2 = 128;
                    multiplier = 23;
                    break;
                case 10:
                    _subnet2 = 192;
                    multiplier = 22;
                    break;
                case 11:
                    _subnet2 = 224;
                    multiplier = 21;
                    break;
                case 12:
                    _subnet2 = 240;
                    multiplier = 20;
                    break;
                case 13:
                    _subnet2 = 248;
                    multiplier = 19;
                    break;
                case 14:
                    _subnet2 = 252;
                    multiplier = 18;
                    break;
                case 15:
                    _subnet2 = 254;
                    multiplier = 17;
                    break;
                #endregion

                #region CIDR Class B
                case 16:
                    _subnet3 = 0;
                    multiplier = 16;
                    break;
                case 17:
                    _subnet3 = 128;
                    multiplier = 15;
                    break;
                case 18:
                    _subnet3 = 192;
                    multiplier = 14;
                    break;
                case 19:
                    _subnet3 = 224;
                    multiplier = 13;
                    break;
                case 20:
                    _subnet3 = 240;
                    multiplier = 12;
                    break;
                case 21:
                    _subnet3 = 248;
                    multiplier = 11;
                    break;
                case 22:
                    _subnet3 = 252;
                    multiplier = 10;
                    break;
                case 23:
                    _subnet3 = 254;
                    multiplier = 9;
                    break;
                #endregion

                #region CIDR Class C
                case 24:
                    _subnet4 = 0;
                    multiplier = 8;
                    break;
                case 25:
                    _subnet4 = 128;
                    multiplier = 7;
                    break;
                case 26:
                    _subnet4 = 192;
                    multiplier = 6;
                    break;
                case 27:
                    _subnet4 = 224;
                    multiplier = 5;
                    break;
                case 28:
                    _subnet4 = 240;
                    multiplier = 4;
                    break;
                case 29:
                    _subnet4 = 248;
                    multiplier = 3;
                    break;
                case 30:
                    _subnet4 = 252;
                    multiplier = 2;
                    break;
                case 31:
                    _subnet4 = 254;
                    multiplier = 1;
                    break;
                #endregion
                default:
                    break;
            }

            if (_cidr >= 8)
            {
                _subnet1 = 255;
            }
            if (_cidr >= 16)
            {
                _subnet2 = 255;
            }
            if (_cidr >= 24)
            {
                _subnet3 = 255;
            }

            DetermineClass();
            PrintSubnetMaskAndCIDR();
            GetWildcardMask();
        }

        public void GetCIDRfromSubnetMask()
        {
            bool validMask = true;

            #region Class A
            if (_subnet1 == 0 && _subnet2 == 0 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 0;
            }
            else if (_subnet1 == 128 && _subnet2 == 0 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 1;
            }
            else if (_subnet1 == 192 && _subnet2 == 0 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 2;
            }
            else if (_subnet1 == 224 && _subnet2 == 0 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 3;
            }
            else if (_subnet1 == 240 && _subnet2 == 0 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 4;
            }
            else if (_subnet1 == 248 && _subnet2 == 0 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 5;
            }
            else if (_subnet1 == 252 && _subnet2 == 0 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 6;
            }
            else if (_subnet1 == 254 && _subnet2 == 0 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 7;
            }
            else if (_subnet1 == 255 && _subnet2 == 0 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 8;
            }
            else if (_subnet1 == 255 && _subnet2 == 128 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 9;
            }
            else if (_subnet1 == 255 && _subnet2 == 192 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 10;
            }
            else if (_subnet1 == 255 && _subnet2 == 224 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 11;
            }
            else if (_subnet1 == 255 && _subnet2 == 240 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 12;
            }
            else if (_subnet1 == 255 && _subnet2 == 248 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 13;
            }
            else if (_subnet1 == 255 && _subnet2 == 252 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 14;
            }
            else if (_subnet1 == 255 && _subnet2 == 254 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 15;
            }
            #endregion

            #region Class B
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 0 && _subnet4 == 0)
            {
                _cidr = 16;
            }
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 128 && _subnet4 == 0)
            {
                _cidr = 17;
            }
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 192 && _subnet4 == 0)
            {
                _cidr = 18;
            }
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 224 && _subnet4 == 0)
            {
                _cidr = 19;
            }
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 240 && _subnet4 == 0)
            {
                _cidr = 20;
            }
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 248 && _subnet4 == 0)
            {
                _cidr = 21;
            }
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 252 && _subnet4 == 0)
            {
                _cidr = 22;
            }
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 254 && _subnet4 == 0)
            {
                _cidr = 23;
            }
            #endregion

            #region Class C
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 255 && _subnet4 == 0)
            {
                _cidr = 24;
            }
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 255 && _subnet4 == 128)
            {
                _cidr = 25;
            }
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 255 && _subnet4 == 192)
            {
                _cidr = 26;
            }
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 255 && _subnet4 == 224)
            {
                _cidr = 27;
            }
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 255 && _subnet4 == 240)
            {
                _cidr = 28;
            }
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 255 && _subnet4 == 248)
            {
                _cidr = 29;
            }
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 255 && _subnet4 == 252)
            {
                _cidr = 30;
            }
            else if (_subnet1 == 255 && _subnet2 == 255 && _subnet3 == 255 && _subnet4 == 254)
            {
                _cidr = 31;
            }
            #endregion
            else
            {
                validMask = false;
            }

            if (validMask == false)
            {
                WrongData();
            }
            else
            {
                GetSubnetMaskFromCIDR();
            }
        }

        public void PrintSubnetMaskAndCIDR()
        {
            if (networkClass == "D" || networkClass == "E")
            {
                OutputSubnetMaskTxt.Text = "Not defined";
                OutputCidrTxt.Text = "N/A";
            }
            else
            {
                OutputSubnetMaskTxt.Text = $"{ _subnet1 }.{ _subnet2 }.{ _subnet3 }.{ _subnet4 }";
                OutputCidrTxt.Text = "/" + _cidr.ToString();
            }
        }

        public void GetWildcardMask()
        {
            wildcardOctet1 = 255 - _subnet1;
            wildcardOctet2 = 255 - _subnet2;
            wildcardOctet3 = 255 - _subnet3;
            wildcardOctet4 = 255 - _subnet4;

            if (networkClass == "D" || networkClass == "E")
            {
                OutputWildcardMaskTxt.Text = "Not defined";
            }
            else
            {
                OutputWildcardMaskTxt.Text = $"{ wildcardOctet1 }.{ wildcardOctet2 }.{ wildcardOctet3 }.{ wildcardOctet4 }";
            }
            Hosts();
        }

        public void Hosts()
        {
            HostExponentiation();
            usableHosts = totalHostsOutput - 2;

            if (networkClass == "D" || networkClass == "E")
            {
                OutputTotalHostsTxt.Text = "Not defined";
                OutputUsableHostsTxt.Text = "Not defined";
            }
            else
            {
                OutputTotalHostsTxt.Text = totalHostsOutput.ToString();
                OutputUsableHostsTxt.Text = usableHosts.ToString();
            }
            GetRange();
        }

        public void HostExponentiation()
        {
            for (double totalHostExponentiation = 0; totalHostExponentiation <= multiplier; totalHostExponentiation++)
            {
                totalHostsOutput = Math.Pow(totalHostsBase, totalHostExponentiation);
            }
        }

        public void GetRange()
        {
            if (_cidr <= 7)
            {
                rangeCalculatorTemp = _subnet1;
                octetTemp = _octet1;
            }
            else if (_cidr <= 15)
            {
                rangeCalculatorTemp = _subnet2;
                octetTemp = _octet2;
            }
            else if (_cidr <= 23)
            {
                rangeCalculatorTemp = _subnet3;
                octetTemp = _octet3;
            }
            else if (_cidr <= 31)
            {
                rangeCalculatorTemp = _subnet4;
                octetTemp = _octet4;
            }
            else { }

            rangeCalculator = substractFromThisToGetRange - rangeCalculatorTemp;

            for (int rangeIpCounter = 0; rangeIpCounter <= octetTemp; rangeIpCounter += rangeCalculator)
            {
                int lowerRangeIpCalculator = rangeIpCounter;
                int higherRangeIpCalculator = rangeIpCounter + rangeCalculator;

                lowerIP = lowerRangeIpCalculator;
                higherIP = higherRangeIpCalculator - 1;
            }

            firstUsable = one.ToString();
            lastUsable = twoFiftyFour.ToString();

            RangeOutput();
        }

        public void RangeOutput()
        {
            string lowerOutput = lowerIP.ToString();
            string higherOutput = higherIP.ToString();
            if (_cidr <= 7)
            {
                OutputNetworkAddressTxt.Text = $"{ lowerOutput }.{ zero }.{ zero }.{ zero }";
                OutputFirstUsableHostTxt.Text = $"{ lowerOutput }.{ zero }.{ zero }.{ firstUsable }";
                OutputLastUsableHostTxt.Text = $"{ higherOutput }.{ twoFiftyFive }.{ twoFiftyFive }.{ lastUsable }";
                OutputBroadcastAddressTxt.Text = $"{ higherOutput }.{ twoFiftyFive }.{ twoFiftyFive }.{ twoFiftyFive }";
            }
            else if (_cidr <= 15)
            {
                OutputNetworkAddressTxt.Text = $"{ inputOctet1 }.{ lowerOutput }.{ zero }.{ zero }";
                OutputFirstUsableHostTxt.Text = $"{ inputOctet1 }.{ lowerOutput }.{ zero }.{ firstUsable }";
                OutputLastUsableHostTxt.Text = $"{ inputOctet1 }.{ higherOutput }.{ twoFiftyFive }.{ lastUsable }";
                OutputBroadcastAddressTxt.Text = $"{inputOctet1 }.{ higherOutput }.{ twoFiftyFive }.{ twoFiftyFive }";
            }
            else if (_cidr <= 23)
            {
                OutputNetworkAddressTxt.Text = $"{inputOctet1 }.{ inputOctet2 }.{ lowerOutput }.{ zero }";
                OutputFirstUsableHostTxt.Text = $"{ inputOctet1 }.{ inputOctet2 }.{ lowerOutput }.{ firstUsable }";
                OutputLastUsableHostTxt.Text = $"{ inputOctet1 }.{ inputOctet2 }.{ higherOutput }.{ lastUsable }";
                OutputBroadcastAddressTxt.Text = $"{ inputOctet1 }.{ inputOctet2 }.{ higherOutput }.{ twoFiftyFive }";
            }
            else if (_cidr <= 30)
            {
                int firstUsableTemp = lowerIP + 1;
                firstUsable = firstUsableTemp.ToString();
                int lastUsableTemp = higherIP - 1;
                lastUsable = lastUsableTemp.ToString();

                OutputNetworkAddressTxt.Text = $"{ inputOctet1 }.{ inputOctet2 }.{ inputOctet3 }.{ lowerOutput }";
                OutputFirstUsableHostTxt.Text = $"{ inputOctet1 }.{ inputOctet2 }.{ inputOctet3 }.{ firstUsable }";
                OutputLastUsableHostTxt.Text = $"{ inputOctet1 }.{ inputOctet2 }.{ inputOctet3 }.{ lastUsable }";
                OutputBroadcastAddressTxt.Text = $"{ inputOctet1 }.{ inputOctet2 }.{ inputOctet3 }.{ higherOutput }";
            }
            else
            {
                OutputNetworkAddressTxt.Text = $"{ inputOctet1 }.{ inputOctet2 }.{ inputOctet3 }.{ lowerOutput }";
                OutputFirstUsableHostTxt.Text = "No hosts available";
                OutputLastUsableHostTxt.Text = "No hosts available";
                OutputBroadcastAddressTxt.Text = $"{ inputOctet1 }.{ inputOctet2 }.{ inputOctet3 }.{ higherOutput }";
            }

            if (networkClass == "D")
            {
                OutputNetworkAddressTxt.Text = "224.0.0.0";
                OutputFirstUsableHostTxt.Text = "Not defined";
                OutputLastUsableHostTxt.Text = "Not defined";
                OutputBroadcastAddressTxt.Text = "239.255.255.255";
            }
            else if (networkClass == "E")
            {
                OutputNetworkAddressTxt.Text = "240.0.0.0";
                OutputFirstUsableHostTxt.Text = "Not defined";
                OutputLastUsableHostTxt.Text = "Not defined";
                OutputBroadcastAddressTxt.Text = "255.255.255.255";
            }
            BinarySubnetMask();
        }

        public void BinarySubnetMask()
        {
            string byte1 = Convert.ToString(_subnet1, 2);
            string byte2 = Convert.ToString(_subnet2, 2);
            string byte3 = Convert.ToString(_subnet3, 2);
            string byte4 = Convert.ToString(_subnet4, 2);
            OutputBinarySubnetMaskTxt.Text = $"{ byte1 }.{ byte2 }.{ byte3 }.{ byte4 }";

            if (networkClass == "D" || networkClass == "E")
            {
                OutputBinarySubnetMaskTxt.Text = "Not defined";
            }
            MessageTxt.Text = complete;
        }

        private void BtnRandom_Click(object sender, RoutedEventArgs e)
        {
            ClearCalculation();
            RandomIpGenerator();
        }

        private void RandomIpGenerator()
        {
            inputOctet1 = random.Next(0, 256).ToString();
            inputOctet2 = random.Next(0, 256).ToString();
            inputOctet3 = random.Next(0, 256).ToString();
            inputOctet4 = random.Next(0, 256).ToString();
            inputCIDR = random.Next(0, 32).ToString();

            Checkbox1CIDR.IsChecked = true;
            //CheckInputOctet1();

            _octet1 = CheckInput(inputOctet1);
            _octet2 = CheckInput(inputOctet2);
            _octet3 = CheckInput(inputOctet3);
            _octet4 = CheckInput(inputOctet4);

            CheckValuesIpOctets();
        }

        private void ClearCalculation()
        {
            totalHostsBase = 2;
            multiplier = 0;
            octetTemp = 0;
            lowerIP = 0;
            higherIP = 0;
            _subnet1 = 0;
            _subnet2 = 0;
            _subnet3 = 0;
            _subnet4 = 0;

            MessageTxt.Background = Brushes.White;
            MessageTxt.Text = clear;
            OutputClassTxt.Text = clear;
            OutputNetworkAddressTxt.Text = clear;
            OutputFirstUsableHostTxt.Text = clear;
            OutputLastUsableHostTxt.Text = clear;
            OutputBroadcastAddressTxt.Text = clear;
            OutputTotalHostsTxt.Text = clear;
            OutputUsableHostsTxt.Text = clear;
            OutputSubnetMaskTxt.Text = clear;
            OutputWildcardMaskTxt.Text = clear;
            OutputBinarySubnetMaskTxt.Text = clear;
            OutputCidrTxt.Text = clear;
        }

        private void Checkbox1CIDR_Checked(object sender, RoutedEventArgs e)
        {
            Checkbox2Netmask.IsChecked = false;
            Checkbox3Wildcard.IsChecked = false;

            CIDRinputTxt.IsReadOnly = false;
        }

        private void Checkbox2Netmask_Checked(object sender, RoutedEventArgs e)
        {
            Checkbox1CIDR.IsChecked = false;
            Checkbox3Wildcard.IsChecked = false;

            Subnet1InputTxt.IsReadOnly = false;
            Subnet2InputTxt.IsReadOnly = false;
            Subnet3InputTxt.IsReadOnly = false;
            Subnet4InputTxt.IsReadOnly = false;
        }

        private void Checkbox3Wildcard_Checked(object sender, RoutedEventArgs e)
        {
            Checkbox1CIDR.IsChecked = false;
            Checkbox2Netmask.IsChecked = false;

            Wildcard1InputTxt.IsReadOnly = false;
            Wildcard2InputTxt.IsReadOnly = false;
            Wildcard3InputTxt.IsReadOnly = false;
            Wildcard4InputTxt.IsReadOnly = false;
        }

        private void BtnClearInput_Click(object sender, RoutedEventArgs e)
        {
            Octet1InputTxt.Text = "";
            Octet2InputTxt.Text = "";
            Octet3InputTxt.Text = "";
            Octet4InputTxt.Text = "";
            Subnet1InputTxt.Text = "";
            Subnet2InputTxt.Text = "";
            Subnet3InputTxt.Text = "";
            Subnet4InputTxt.Text = "";
            Wildcard1InputTxt.Text = "";
            Wildcard2InputTxt.Text = "";
            Wildcard3InputTxt.Text = "";
            Wildcard4InputTxt.Text = "";
            CIDRinputTxt.Text = "";
        }

        private void WrongData()
        {
            MessageTxt.Text = "Wrong Data Input";
            MessageTxt.Background = Brushes.Red;
        }
    }
}
