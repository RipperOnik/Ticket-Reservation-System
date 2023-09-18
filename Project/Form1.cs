using System.Diagnostics;

namespace Project
{
    public partial class Form1 : Form
    {
        List<Seat> seats = new List<Seat>();
        Seat? selectedSeat = null;
        public Form1()
        {
            InitializeComponent();
            initializeSeats();
            reserveUserMessage.Text = "";
            unreserveNameUserMessage.Text = "";
            unreserveSeatNumberUserMessage.Text = "";
            userMessage.Text = "";
        }
        private bool areSeatsAvailable()
        {
            foreach (Seat seat in seats)
            {
                // if at least one seat is available
                if (!seat.isReserved())
                {
                    return true;
                }
            }
            // if not, return false
            return false;
        }


        private void initializeSeats()
        {

            foreach (Control seatControl in tableLayoutPanel1.Controls)
            {
                if (seatControl != null)
                {
                    Label label = seatControl as Label;
                    if (label != null)
                    {
                        Seat seat = new Seat(int.Parse(label.Text), seatControl);

                        seats.Add(seat);
                    }
                }
            }
            Trace.WriteLine(seats.Count);
        }



        private void label1_Click(object sender, EventArgs e)
        {

            Control clickedCell = sender as Control;
            if (clickedCell != null)
            {

                Seat chosenSeat = seats.Find(s => s.seatControl == clickedCell);

                // unselect previous selected seat
                if (selectedSeat != null)
                {
                    selectedSeat.unselectSeat();
                    selectedSeat = null;
                }
                // select new seat
                if (chosenSeat != null && !chosenSeat.isReserved())
                {
                    selectedSeat = chosenSeat;
                    selectedSeat.selectSeat();
                    Trace.WriteLine(selectedSeat.seatNumber.ToString());
                }
            }

        }

        private void reserveButton_Click(object sender, EventArgs e)
        {
            if (reserveCustomerNameTextBox.Text.Length > 0)
            {
                if (selectedSeat != null)
                {
                    // reserve seat
                    selectedSeat.reserveSeat(reserveCustomerNameTextBox.Text);
                    selectedSeat = null;
                    reserveCustomerNameTextBox.Text = "";
                    reserveUserMessage.ForeColor = Color.Green;
                    reserveUserMessage.Text = "Successfully reserved a seat!";
                    // inform user if no seats are available
                    if (!areSeatsAvailable())
                    {
                        userMessage.Text = "No seats are available";
                    }
                }
                else
                {
                    //display not selected error
                    reserveUserMessage.ForeColor = Color.Red;
                    reserveUserMessage.Text = "Select a seat";
                }

            }
            else
            {
                // display empty error
                reserveUserMessage.ForeColor = Color.Red;
                reserveUserMessage.Text = "Enter customer name";

            }

        }


        private void unreserveNameButton_Click(object sender, EventArgs e)
        {
            String customerName = unreserveCustomerNameTextBox.Text;
            if (customerName.Length > 0)
            {
                bool unreservedAtLeastOne = false;
                foreach (Seat seat in seats)
                {
                    if (seat.customerName == customerName && seat.isReserved())
                    {
                        unreservedAtLeastOne = true;
                        seat.unreserveSeat();

                    }
                }
                if (unreservedAtLeastOne)
                {
                    // display incorrect seat number
                    unreserveNameUserMessage.ForeColor = Color.Green;
                    unreserveNameUserMessage.Text = "Successfully unreserved seats for this customer name!";


                }
                else
                {
                    // display incorrect seat number
                    unreserveNameUserMessage.ForeColor = Color.Red;
                    unreserveNameUserMessage.Text = "No seat found for this name";

                }
                unreserveCustomerNameTextBox.Text = "";


            }
            else
            {
                // display empty name error
                unreserveNameUserMessage.ForeColor = Color.Red;
                unreserveNameUserMessage.Text = "Enter customer name";
            }

        }

        private void unreserveSeatNumberButton_Click(object sender, EventArgs e)
        {
            int? chosenSeatNumber = (int)unreserveSeatNumericBox.Value;
            if (chosenSeatNumber != null)
            {
                Seat chosenSeat = seats.Find(s => s.seatNumber == chosenSeatNumber);
                if (chosenSeat != null)
                {
                    if (chosenSeat.isReserved())
                    {
                        // unreserve seat
                        chosenSeat.unreserveSeat();

                        unreserveSeatNumberUserMessage.ForeColor = Color.Green;
                        unreserveSeatNumberUserMessage.Text = "Successfully unreserved a seat!";
                        unreserveSeatNumericBox.Value = 0;

                    }
                    else
                    {
                        // display error if seat is not reserved
                        unreserveSeatNumberUserMessage.ForeColor = Color.Red;
                        unreserveSeatNumberUserMessage.Text = "This seat has not been reserved";

                    }



                }
                else
                {
                    // display error if seat number is out of range
                    unreserveSeatNumberUserMessage.ForeColor = Color.Red;
                    unreserveSeatNumberUserMessage.Text = "Enter a seat number in range (1,25)";

                }

            }
            else
            {
                // display error if seat number is null
                unreserveSeatNumberUserMessage.ForeColor = Color.Red;
                unreserveSeatNumberUserMessage.Text = "Enter a seat number";
            }
        }
    }
    public class Seat
    {
        public int seatNumber;
        public String? customerName;
        public Control seatControl;

        public Seat(int seatNumber, Control seatControl)
        {
            this.seatNumber = seatNumber;
            this.seatControl = seatControl;
            this.customerName = null;
        }

        public void selectSeat()
        {
            if (!isReserved())
            {
                seatControl.BackColor = Color.Green;
            }
        }
        public void unselectSeat()
        {

            seatControl.BackColor = Color.Transparent;

        }
        public void reserveSeat(String customerName)
        {
            if (!isReserved())
            {
                this.customerName = customerName;
                seatControl.BackColor = Color.Gray;
            }
        }
        public void unreserveSeat()
        {
            if (isReserved())
            {
                customerName = null;
                seatControl.BackColor = Color.Transparent;
            }
        }

        public bool isReserved()
        {
            return (customerName != null);
        }


    }
}