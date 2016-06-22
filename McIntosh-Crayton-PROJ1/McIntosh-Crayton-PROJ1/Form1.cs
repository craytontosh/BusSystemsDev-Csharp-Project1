/* Developer: McIntosh, Crayton
 * Course: MIS 4321 - Spring 2016
 * Assignment: Project #1 - Vitals Dashboard
 * Description: This program will prompt the user for body composition data. It will calculate the heart rate, BMI, Glucose level, 
 *              blood pressure (both systolic and diastolic), and display all this data in a user friendly UI. 
 *              This program will also change indicator color if you are above a certain BMI, glucose level, and blood pressure.
 * 
 * Bonus #1: SetOverallHypertensionLevel() - YES
 * Bonus #2: Innovative Feature - Theme Change YES; This feature changes the color scheme of the form, 
 *                                making it a dark themed 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace McIntosh_Crayton_PROJ1
{
    public partial class VitalsForm : Form
    {
        public VitalsForm()
        {
            InitializeComponent();
        }

        private void VitalsForm_Load(object sender, EventArgs e)
        {
            //loads sample data and hides the other panels from view
            LoadTestData();
            DisplayDashboard(false);
        }

        //INNOVATIVE FEATURE
        /*bonus*/ private void btnTurnOffLights_Click(object sender, EventArgs e)
        {   
            //if you want to turn the lights off and make the program easier to look at with a darker theme
            if(btnTurnOffLights.Text == "Turn Off the Lights")
            {
                //changes the text color
                lblHeartRateTitle.ForeColor = Color.White;
                lblBloodPressurePanel.ForeColor = Color.White;
                lblMaxHeartRateLabel.ForeColor = Color.White;
                lblHeartRateBpm.ForeColor = Color.White;
                lblMinLabel.ForeColor = Color.White;
                lblMaxLabel.ForeColor = Color.White;
                lblBpmMinLabel.ForeColor = Color.White;
                lblBpmMaxLabel.ForeColor = Color.White;
                lblRecomendations.ForeColor = Color.White;
                lblAndLabel.ForeColor = Color.White;
                lblInTenSeconds.ForeColor = Color.White;
                lblDoNotExceedLabel.ForeColor = Color.White;

                //changes the panel colors to a dark color
                pnlBMI.BackColor = Color.FromArgb(64, 64, 64);
                pnlHeartRate1.BackColor = Color.FromArgb(64, 64, 64);
                pnlHeartRate2.BackColor = Color.FromArgb(64, 64, 64);
                pnlBloodPressure.BackColor = Color.FromArgb(64, 64, 64);
                pnlGlucose.BackColor = Color.FromArgb(64, 64, 64);
               
                //changes the text of the button that changes the theme
                btnTurnOffLights.Text = "Turn On the Lights";
            }
            //if you want to turn the lights back ON to revert the color scheme back to the original
            else if (btnTurnOffLights.Text == "Turn On the Lights")
            {
                //changes the text color back to the original
                lblVitalsDashboardTitle.ForeColor = Color.FromArgb(198, 172, 16);
                lblHeartRateTitle.ForeColor = Color.FromArgb(126, 96, 0);
                lblBloodPressurePanel.ForeColor = Color.Black; 
                lblMaxHeartRateLabel.ForeColor = Color.Black;
                lblHeartRateBpm.ForeColor = Color.Black;
                lblMinLabel.ForeColor = Color.Black;
                lblMaxLabel.ForeColor = Color.Black;
                lblBpmMinLabel.ForeColor = Color.Black;
                lblBpmMaxLabel.ForeColor = Color.Black;
                lblRecomendations.ForeColor = Color.Black;
                lblAndLabel.ForeColor = Color.Black;
                lblInTenSeconds.ForeColor = Color.Black;
                lblDoNotExceedLabel.ForeColor = Color.Black;

                //changes the panel color back to the original
                pnlBMI.BackColor = Color.FromArgb(126, 96, 0);
                pnlHeartRate1.BackColor = Color.FromArgb(198, 172, 16);
                pnlHeartRate2.BackColor = Color.White;
                pnlBloodPressure.BackColor = Color.White;
                pnlGlucose.BackColor = Color.FromArgb(126, 96, 0);
                
                //changes the text of the button that changes the theme
                btnTurnOffLights.Text = "Turn Off the Lights";
            }
        }

        private void btnSubmitVitals_Click(object sender, EventArgs e)
        {
            //resets color of labels 
            ResetHypertensionColor();

            //checks if the textboxs are empty or not
            if (string.IsNullOrEmpty(txtAge.Text) || string.IsNullOrEmpty(txtHeightFeet.Text) || 
                string.IsNullOrEmpty(txtHeightInches.Text) || string.IsNullOrEmpty(txtWeight.Text) ||
                string.IsNullOrEmpty(txtBloodPressureSYS.Text) || string.IsNullOrEmpty(txtBloodPressureDIA.Text) || 
                string.IsNullOrEmpty(txtGlucose.Text))
            {
                MessageBox.Show("Please Enter Your Information");
                DisplayDashboard(false);
            }
            else
            {
                //if the textboxes aren't emply and contain correct information, the program will run these methods
                CalcHeartRate();
                CalcBMI();
                CalcGlucose();
                CalcBloodPressureSystolic();
                CalcBloodPressureDiastolic();
                DisplayDashboard(true);
                SetOverallHypertensionLevel();
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            //clears each text box and all data
            txtAge.Clear();
            txtHeightFeet.Clear();
            txtHeightInches.Clear();
            txtWeight.Clear();
            txtBloodPressureSYS.Clear();
            txtBloodPressureDIA.Clear();
            txtGlucose.Clear();
            DisplayDashboard(false);
            ResetHypertensionColor();
        }

        private void btnLoadTestData_Click(object sender, EventArgs e)
        {
            //loads test data
            LoadTestData();
        }

        private void LoadTestData()
        {
            txtAge.Text = "20";
            txtHeightFeet.Text = "5";
            txtHeightInches.Text = "6";
            txtWeight.Text = "170.5";
            txtBloodPressureSYS.Text = "150";
            txtBloodPressureDIA.Text = "70";
            txtGlucose.Text = "90";
            DisplayDashboard(false);
        }
        
        private void DisplayDashboard(bool bVisible)
        {
            //changes the visibility of each panel
            pnlBloodPressure.Visible = bVisible;
            pnlBMI.Visible = bVisible;
            pnlGlucose.Visible = bVisible;
            pnlHeartRate1.Visible = bVisible;
            pnlHeartRate2.Visible = bVisible;
        }
        
        private void CalcHeartRate()
        {
            //data abstraction and process
            int maxHeartRate = 220 - Convert.ToInt32(txtAge.Text);
            double minTarget = (maxHeartRate * 0.5);
            double maxTarget = (maxHeartRate * 0.85);
            double minTargetEveryTenSeconds = minTarget / 6;
            double maxTargetEveryTenSeconds = maxTarget / 6;
            double doNotExceed = maxHeartRate / 6; 

            //output for hearbeat panels
            lblMaxHeartRateNumber.Text = Convert.ToString(maxHeartRate);
            lblTargetZoneMin.Text = minTarget.ToString("N0");
            lblTargetZoneMax.Text = maxTarget.ToString("N0");
            lblTurqMin.Text = minTargetEveryTenSeconds.ToString("N0");
            lblTurqMax.Text = maxTargetEveryTenSeconds.ToString("N0");
            lblRedDoNotExceed.Text = doNotExceed.ToString("N0");
        }
        
        private void CalcBMI()
        {
            //data declaration and process
            const double BMI_CALC_INDEX = 703;
            double weight = Convert.ToDouble(txtWeight.Text);
            double height = (Convert.ToDouble(txtHeightFeet.Text) * 12) + Convert.ToDouble(txtHeightInches.Text);
            double BMI = ((weight * BMI_CALC_INDEX) / height) / height;

            //output
            lblBmiNumber.Text = BMI.ToString("N1");

            //if statements determining where to put the BMI indicator
            
            //Underweight
            if (BMI < 18.5)
            {
                //changes the color of the indicator to gold
                this.picIndicatorBmi.BackColor = Color.Gold;
                //changes the location of the indicator
                int anIntX = picBmiUnder.Location.X + 5;
                int anIntY = picBmiUnder.Location.Y + 5;

                picIndicatorBmi.Location = new Point(anIntX, anIntY);
            }

            //Healthy
            else if (BMI >= 18.5 && BMI <= 24.9)
            {
                //changes the color of the indicator to gold
                this.picIndicatorBmi.BackColor = Color.Gold;
                //changes the location of the indicator
                int anIntX = picBmiHealthy.Location.X + 5;
                int anIntY = picBmiHealthy.Location.Y + 5;

                picIndicatorBmi.Location = new Point(anIntX, anIntY);
            }

            //Overweight
            else if (BMI >= 25.0 && BMI <= 29.9)
            { 
                //changes the color of the indicator to red
                this.picIndicatorBmi.BackColor = Color.Firebrick;
                //changes the location of the indicator
                int anIntX = picBmiOver.Location.X + 5;
                int anIntY = picBmiOver.Location.Y + 5;

                picIndicatorBmi.Location = new Point(anIntX, anIntY);
            }

            //Obese
            else if (BMI >= 30.0 && BMI <= 39.9)
            {
                //changes the color of the indicator to red
                this.picIndicatorBmi.BackColor = Color.Firebrick;
                //changes the location of the indicator
                int anIntX = picBmiObese.Location.X + 5;
                int anIntY = picBmiObese.Location.Y + 5;

                picIndicatorBmi.Location = new Point(anIntX, anIntY);
            }

            //Extreme or high risk obesity
            else
            {
                //changes the color of the indicator to red
                this.picIndicatorBmi.BackColor = Color.Firebrick;
                //changes the location of the indicator
                int anIntX = picBmiHigh.Location.X + 5;
                int anIntY = picBmiHigh.Location.Y + 5;

                picIndicatorBmi.Location = new Point(anIntX, anIntY);
            }
        }

        private void CalcGlucose()
        {
            //data declaration and input
            int glucoseLevel = Convert.ToInt32(txtGlucose.Text);

            //process
            if (glucoseLevel > 0 && glucoseLevel <= 69)
            {
                this.picIndicatorGlucose.BackColor = Color.Gold;

                //changes the location of the indicator
                int anIntX = picGlucoseLow.Location.X + 5;
                int anIntY = picGlucoseLow.Location.Y + 5;

                picIndicatorGlucose.Location = new Point(anIntX, anIntY);
            }
            else if (glucoseLevel >= 70 && glucoseLevel <= 99)
            {
                this.picIndicatorGlucose.BackColor = Color.Gold;

                //changes the location of the indicator
                int anIntX = picGlucoseNormal.Location.X + 5;
                int anIntY = picGlucoseNormal.Location.Y + 5;

                picIndicatorGlucose.Location = new Point(anIntX, anIntY);
            }
            else if (glucoseLevel >= 100 && glucoseLevel <= 125)
            {
                this.picIndicatorGlucose.BackColor = Color.Firebrick;

                //changes the location of the indicator
                int anIntX = picGlucosePreDiabetes.Location.X + 5;
                int anIntY = picGlucosePreDiabetes.Location.Y + 5;

                picIndicatorGlucose.Location = new Point(anIntX, anIntY);
            }
            else
            {
                this.picIndicatorGlucose.BackColor = Color.Firebrick;

                //changes the location of the indicator
                int anIntX = picGlucoseDiabetes.Location.X + 5;
                int anIntY = picGlucoseDiabetes.Location.Y + 5;

                picIndicatorGlucose.Location = new Point(anIntX, anIntY);
            }

            //output
            lblGlucoseNumber.Text = Convert.ToString(txtGlucose.Text);
        }

        private void CalcBloodPressureSystolic()
        {
            //data declaration, input, and partial process
            const int SYSTOLIC_BP_MAX = 200; 
            const int TOTAL_WIDTH = 400;
            int systolicBloodPressure = Convert.ToInt32(txtBloodPressureSYS.Text);
            int shiftAmt = (TOTAL_WIDTH * systolicBloodPressure) / SYSTOLIC_BP_MAX;
            int anIntX = 35 + shiftAmt;
            int anIntY = picIndicatorBPsystolic.Location.Y;

            //process for color change and indicator location
            if (systolicBloodPressure < 120)
            {
                picIndicatorBPsystolic.Location = new Point(anIntX , anIntY);
                picIndicatorBPsystolic.BackColor = Color.Gold;
            }
            else if (systolicBloodPressure < 140)
            {
                picIndicatorBPsystolic.Location = new Point(anIntX , anIntY);
                picIndicatorBPsystolic.BackColor = Color.Firebrick;
            }
            else if (systolicBloodPressure < 160)
            {
                picIndicatorBPsystolic.Location = new Point(anIntX, anIntY);
                picIndicatorBPsystolic.BackColor = Color.Firebrick;
            }
            else
            {
                picIndicatorBPsystolic.Location = new Point(anIntX, anIntY);
                picIndicatorBPsystolic.BackColor = Color.Gold;
            }
            //output
            lblBloodPressureSystolicNumber.Text = txtBloodPressureSYS.Text;
        }

        private void CalcBloodPressureDiastolic()
        {            
            //data declaration, input, and partial process
            const int DIASTOLIC_BP_MAX = 133;
            const int TOTAL_WIDTH = 400;
            int diastolicBloodPressure = Convert.ToInt32(txtBloodPressureDIA.Text);
            int shiftAmt = (TOTAL_WIDTH * diastolicBloodPressure) / DIASTOLIC_BP_MAX;
            int anIntX = 35 + shiftAmt;
            int anIntY = picIndicatorBPdiastolic.Location.Y;

            //process for color change and indicator location
            if (diastolicBloodPressure < 80)
            {
                picIndicatorBPdiastolic.Location = new Point(anIntX, anIntY);
                picIndicatorBPdiastolic.BackColor = Color.Gold;
            }
            else if (diastolicBloodPressure < 90)
            {
                picIndicatorBPdiastolic.Location = new Point(anIntX, anIntY);
                picIndicatorBPdiastolic.BackColor = Color.Firebrick;
            }
            else if (diastolicBloodPressure < 100)
            {
                picIndicatorBPdiastolic.Location = new Point(anIntX, anIntY);
                picIndicatorBPdiastolic.BackColor = Color.Firebrick;
            }
            else
            {
                picIndicatorBPdiastolic.Location = new Point(anIntX, anIntY);
                picIndicatorBPdiastolic.BackColor = Color.Firebrick;
            }

            //output
            lblBloodPressureDiastolicNumber.Text = txtBloodPressureDIA.Text;
        }

        private void SetOverallHypertensionLevel()
        {
            //input and data declaration
            int sys = Convert.ToInt32(txtBloodPressureSYS.Text);
            int dia = Convert.ToInt32(txtBloodPressureDIA.Text);
            
            //Process and output changing the colors of the words
            //systolic
            if (sys < 120)
            {
                lblBloodPressureNormal.ForeColor = Color.Gold;
            }
            else if (sys < 140)
            {
                lblBloodPressurePre.ForeColor = Color.Firebrick;
            }
            else if (sys < 160)
            {
                lblBloodPressureStage1.ForeColor = Color.Firebrick;
            }
            else
            {
                lblBloodPressureStage2.ForeColor = Color.Firebrick;
            }

            //diastolic
            if (dia < 80)
            {
                lblBloodPressureNormal.ForeColor = Color.Gold;
            }
            else if (dia < 90)
            {
                lblBloodPressurePre.ForeColor = Color.Firebrick;
            }
            else if (dia < 100)
            {
                lblBloodPressureStage1.ForeColor = Color.Firebrick;
            }
            else
            {
                lblBloodPressureStage2.ForeColor = Color.Firebrick;
            }
        }

        private void ResetHypertensionColor()
        {
            //resets the colors to grey upon being called
            lblBloodPressureNormal.ForeColor = Color.Gray;
            lblBloodPressurePre.ForeColor = Color.Gray;
            lblBloodPressureStage1.ForeColor = Color.Gray;
            lblBloodPressureStage2.ForeColor = Color.Gray;
        }

    }
}
