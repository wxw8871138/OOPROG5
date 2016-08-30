using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace OOPROG5
{
    class Program
    {
        static void Main(string[] args)
        {
            Customer a = new Customer("wang", "clementi", "xxx", new DateTime(1993, 9, 18));
            Customer b = new Customer("zhang", "ISS", "xxf", new DateTime(1999, 2, 8));
            Customer c = new Customer("pan", "home", "xxz", new DateTime(1993, 11, 1));
            SavingAccount x = new SavingAccount("000-000-001", a, 2000);
            CurrentAccount y = new CurrentAccount("000-000-002", b, 20);
            OverDraftAccount z = new OverDraftAccount("000-000-003", c, 50000);

            ArrayList list = new ArrayList();
            list.Add(a);
            list.Add(x);
            Console.WriteLine("Before creditinterest:");
            Console.WriteLine(x.ToString());
            Console.WriteLine(y.ToString());
            Console.WriteLine(z.ToString());

            x.CreditInterest();
            y.CreditInterest();
            z.CreditInterest();
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("After creditinterest:");
            Console.WriteLine(x.ToString());
            Console.WriteLine(y.ToString());
            Console.WriteLine(z.ToString());

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("After pan withdraw 100000 & creditinterest:");
            z.Withdraw(100000);
            z.CreditInterest();
            Console.WriteLine(z.ToString());

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("BankBranch:");
            BankBranch bankbranch = new BankBranch("OCBC","wxw");
            bankbranch.AddAccount(x);
            bankbranch.AddAccount(y);
            bankbranch.AddAccount(z);
            Console.WriteLine(bankbranch);
        }
    }
    class Customer
    {
        private string name;
        private string address;
        private string passportNo;
        private DateTime dateOfBirth;

        public Customer()
        {

        }
        public Customer(string name, string address, string passportNo, DateTime dateOfBirth)
        {
            this.name = name;
            this.address = address;
            this.passportNo = passportNo;
            this.dateOfBirth = dateOfBirth;
        }

        public int GetAge()
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            return age;
        }
        public string GetName()
        {
            return name;
        }

        public string GetAddress()
        {
            return address;
        }
    }
    class BankAccount3
    {
        string number;
        Customer holder;
        double balance;
        double interestRate = 0;

        public BankAccount3(string number, Customer holder, double balance)
        {
            this.number = number;
            this.holder = holder;
            this.balance = balance;
        }
        public virtual bool Withdraw(double amount)
        {
            if (amount <= balance)
            {
                balance = balance = amount;
                return true;
            }
            else
            {
                Console.Error.WriteLine("Withdraw for {0} is unseccessful.No enough balance", holder.GetName());
                return false;
            }
        }
        public void Deposit(double amount)
        {
            balance = balance + amount;
        }

        public virtual bool TransferTo(double amount, BankAccount3 another)
        {
            if (Withdraw(amount))
            {
                another.Deposit(amount);
                return true;
            }
            else
            {
                Console.Error.WriteLine("Translate for {0} is unseccessful.No enough balance", holder.GetName());
                return false;
            }
        }

        public virtual double CalculateInterest()
        {
            double insterest;
            insterest = Balance * interestRate;
            return insterest;
        }

        public void CreditInterest()
        {
            balance = balance + CalculateInterest();
        }

        public double Balance
        {
            get
            {
                return balance;
            }
            set
            {
                balance = value;
            }
        }

        public override string ToString()
        {
            string info;
            info = string.Format("number={0},holder={1},balance={2}", number, holder.GetName(), balance);
            return info;
        }

    }
    class SavingAccount : BankAccount3
    {
        double interestRate = 0.01;

        public SavingAccount(string number, Customer holder, double balance) : base(number, holder, balance)
        {
        }

        public override double CalculateInterest()
        {
            double interest;
            interest = Balance * interestRate;
            return interest;
        }

        public new void CreditInterest()
        {
            Balance = Balance + CalculateInterest();
        }
    }

    class CurrentAccount : BankAccount3
    {
        double interestRate = 0.0025;

        public CurrentAccount(string number, Customer holder, double balance) : base(number, holder, balance)
        {
        }

        public override double CalculateInterest()
        {
            double interest;
            interest = Balance * interestRate;
            return interest;
        }

        public new void CreditInterest()
        {
            Balance = Balance + CalculateInterest();
        }
    }

    class OverDraftAccount : BankAccount3
    {
        double interestRate = 0.0025;
        double interestRateNeg = 0.06;

        public OverDraftAccount(string number, Customer holder, double balance) : base(number, holder, balance)
        {
        }
        public override bool Withdraw(double amount)
        {
            Balance = Balance - amount;
            return true;
        }
        public override double CalculateInterest()
        {
            double interest;
            if (Balance > 0)
            {
                interest = Balance * interestRate;
            }
            else
            {
                interest = Balance * interestRateNeg;
            }
            return interest;
        }

        public new void CreditInterest()
        {
            Balance = Balance + CalculateInterest();
        }
    }
    class BankBranch
    {
        string branchName;
        string branchManager;
        ArrayList listOfBankAccount = new ArrayList();

        public BankBranch(string name,string manager)
        {
            branchName = name;
            branchManager = manager;
        }
        public void AddAccount(BankAccount3 account)
        {
            listOfBankAccount.Add(account);
        }

        public void PrintCustomers()
        {
            for (int i = 0; i < listOfBankAccount.Count; i++)
            {
                BankAccount3 account = (BankAccount3)listOfBankAccount[i];
                Console.WriteLine(account);
            }
        }

        public double ToatalDeposts()
        {
            double total = 0;
            for (int i = 0; i < listOfBankAccount.Count; i++)
            {
                BankAccount3 account = (BankAccount3)listOfBankAccount[i];
                total = total + account.Balance;
            }
            return total;
        }

        public double TotalInterestPaid()
        {
            double totalInterest = 0;
            for (int i = 0; i < listOfBankAccount.Count; i++)
            {
                BankAccount3 account = (BankAccount3)listOfBankAccount[i];
                if (account.CalculateInterest() > 0)
                {
                    totalInterest = totalInterest + account.CalculateInterest();
                }

            }
            return totalInterest;
        }
        public double TotalInterestEarned()
        {
            double totalInterest = 0;
            for (int i = 0; i < listOfBankAccount.Count; i++)
            {
                BankAccount3 account = (BankAccount3)listOfBankAccount[i];
                if (account.CalculateInterest() < 0)
                {
                    totalInterest = totalInterest + account.CalculateInterest();
                }

            }
            return -totalInterest;
        }

        public override string ToString()
        {
            return string.Format("BranchName={0},BranchManager={1},TotalDeposits={2},TotalInterestPaid={3},TotalInterestEarned={4}",branchName,branchManager,ToatalDeposts(),TotalInterestPaid(),TotalInterestEarned());
        }



    }
}