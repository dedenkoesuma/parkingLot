using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var parkingLot = new ParkingLot();
            parkingLot.CreateParkingLot(6); 
            string command;
            do
            {
                Console.Write("Enter your commands: ");
                command = Console.ReadLine();
                if (command.StartsWith("park"))
                {
                    string[] details = command.Split(' ');
                    parkingLot.Park(new Vehicle(details[1], details[2], details[3]));
                }
                else if (command.StartsWith("leave"))
                {
                    int slotNumber = int.Parse(command.Split(' ')[1]);
                    parkingLot.Leave(slotNumber);
                }
                else if (command == "status")
                {
                    parkingLot.Status();
                }
                else if (command.StartsWith("type_of_vehicles"))
                {
                    string type = command.Split(' ')[1];
                    parkingLot.TypeOfVehicles(type);
                }
                else if (command.StartsWith("registration_numbers_for_vehicles_with_ood_plate"))
                {
                    parkingLot.RegistrationNumbersForVehiclesWithOddPlate();
                }
                else if (command.StartsWith("registration_numbers_for_vehicles_with_event_plate"))
                {
                    parkingLot.RegistrationNumbersForVehiclesWithEvenPlate();
                }
                else if (command.StartsWith("registration_numbers_for_vehicles_with_colour"))
                {
                    string colour = command.Split(' ')[1];
                    parkingLot.RegistrationNumbersForVehiclesWithColour(colour);
                }
                else if (command.StartsWith("slot_numbers_for_vehicles_with_colour"))
                {
                    string colour = command.Split(' ')[1];
                    parkingLot.SlotNumbersForVehiclesWithColour(colour);
                }
                else if (command.StartsWith("slot_number_for_registration_number"))
                {
                    string registrationNumber = command.Split(' ')[1];
                    parkingLot.SlotNumberForRegistrationNumber(registrationNumber);
                }
            } while (command != "exit");
        }
    }

    class ParkingLot
    {
        private int capacity;
        private List<Slot> slots;

        public ParkingLot()
        {
            slots = new List<Slot>();
        }

        public void CreateParkingLot(int capacity)
        {
            this.capacity = capacity;
            for (int i = 1; i <= capacity; i++)
            {
                slots.Add(new Slot(i));
            }
            Console.WriteLine($"Created a parking lot with {capacity} slots");
        }

        public void Park(Vehicle vehicle)
        {
            var slot = slots.FirstOrDefault(s => s.IsAvailable);
            if (slot != null)
            {
                slot.Park(vehicle);
                Console.WriteLine($"Allocated slot number: {slot.Number}");
            }
            else
            {
                Console.WriteLine("Sorry, parking lot is full");
            }
        }

        public void Leave(int slotNumber)
        {
            var slot = slots.FirstOrDefault(s => s.Number == slotNumber);
            if (slot != null)
            {
                slot.Leave();
                Console.WriteLine($"Slot number {slotNumber} is free");
            }
        }

        public void Status()
        {
            Console.WriteLine("Slot\tNo.\tType\tRegistration No\tColour");
            foreach (var slot in slots.Where(s => !s.IsAvailable))
            {
                Console.WriteLine($"{slot.Number}\t{slot.Vehicle.RegistrationNumber}\t{slot.Vehicle.Type}\t{slot.Vehicle.Colour}");
            }
        }

        public void TypeOfVehicles(string type)
        {
            int count = slots.Count(s => !s.IsAvailable && s.Vehicle.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine(count);
        }

        public void RegistrationNumbersForVehiclesWithOddPlate()
        {
            var registrationNumbers = slots.Where(s => !s.IsAvailable && s.Vehicle.IsOddNumberedPlate).Select(s => s.Vehicle.RegistrationNumber);
            Console.WriteLine(string.Join(", ", registrationNumbers));
        }

        public void RegistrationNumbersForVehiclesWithEvenPlate()
        {
            var registrationNumbers = slots.Where(s => !s.IsAvailable && s.Vehicle.IsEvenNumberedPlate).Select(s => s.Vehicle.RegistrationNumber);
            Console.WriteLine(string.Join(", ", registrationNumbers));
        }

        public void RegistrationNumbersForVehiclesWithColour(string colour)
        {
            var registrationNumbers = slots.Where(s => !s.IsAvailable && s.Vehicle.Colour.Equals(colour, StringComparison.OrdinalIgnoreCase)).Select(s => s.Vehicle.RegistrationNumber);
            Console.WriteLine(string.Join(", ", registrationNumbers));
        }

        public void SlotNumbersForVehiclesWithColour(string colour)
        {
            var slotNumbers = slots.Where(s => !s.IsAvailable && s.Vehicle.Colour.Equals(colour, StringComparison.OrdinalIgnoreCase)).Select(s => s.Number);
            Console.WriteLine(string.Join(", ", slotNumbers));
        }

        public void SlotNumberForRegistrationNumber(string registrationNumber)
        {
            var slot = slots.FirstOrDefault(s => !s.IsAvailable && s.Vehicle.RegistrationNumber.Equals(registrationNumber, StringComparison.OrdinalIgnoreCase));
            if (slot != null)
            {
                Console.WriteLine(slot.Number);
            }
            else
            {
                Console.WriteLine("Not found");
            }
        }
    }

    class Slot
    {
        public int Number { get; private set; }
        public Vehicle Vehicle { get; private set; }
        public bool IsAvailable => Vehicle == null;

        public Slot(int number)
        {
            Number = number;
        }

        public void Park(Vehicle vehicle)
        {
            Vehicle = vehicle;
        }

        public void Leave()
        {
            Vehicle = null;
        }
    }

    class Vehicle
    {
        public string RegistrationNumber { get; private set; }
        public string Colour { get; private set; }
        public string Type { get; private set; }
        public bool IsOddNumberedPlate => RegistrationNumber.Where(char.IsDigit).Sum(c => c - '0') % 2 != 0;
        public bool IsEvenNumberedPlate => !IsOddNumberedPlate;

        public Vehicle(string registrationNumber, string colour, string type)
        {
            RegistrationNumber = registrationNumber;
            Colour = colour;
            Type = type;
        }
    }
}

