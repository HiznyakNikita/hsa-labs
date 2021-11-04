using Microsoft.EntityFrameworkCore;

#nullable disable

namespace HSA.Homework.Web
{
	public partial class AirlinesDbContext : DbContext
    {
        public AirlinesDbContext()
        {
        }

        public AirlinesDbContext(DbContextOptions<AirlinesDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aircraft> Aircrafts { get; set; }
        public virtual DbSet<AircraftsDatum> AircraftsData { get; set; }
        public virtual DbSet<Airport> Airports { get; set; }
        public virtual DbSet<AirportsDatum> AirportsData { get; set; }
        public virtual DbSet<BoardingPass> BoardingPasses { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<FlightsV> FlightsVs { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<Seat> Seats { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketFlight> TicketFlights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<Aircraft>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("aircrafts", "bookings");

                entity.HasComment("Aircrafts");

                entity.Property(e => e.AircraftCode)
                    .HasMaxLength(3)
                    .HasColumnName("aircraft_code")
                    .IsFixedLength(true)
                    .HasComment("Aircraft code, IATA");

                entity.Property(e => e.Model)
                    .HasColumnName("model")
                    .HasComment("Aircraft model");

                entity.Property(e => e.Range)
                    .HasColumnName("range")
                    .HasComment("Maximal flying distance, km");
            });

            modelBuilder.Entity<AircraftsDatum>(entity =>
            {
                entity.HasKey(e => e.AircraftCode)
                    .HasName("aircrafts_pkey");

                entity.ToTable("aircrafts_data", "bookings");

                entity.HasComment("Aircrafts (internal data)");

                entity.Property(e => e.AircraftCode)
                    .HasMaxLength(3)
                    .HasColumnName("aircraft_code")
                    .IsFixedLength(true)
                    .HasComment("Aircraft code, IATA");

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasColumnType("jsonb")
                    .HasColumnName("model")
                    .HasComment("Aircraft model");

                entity.Property(e => e.Range)
                    .HasColumnName("range")
                    .HasComment("Maximal flying distance, km");
            });

            modelBuilder.Entity<Airport>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("airports", "bookings");

                entity.HasComment("Airports");

                entity.Property(e => e.AirportCode)
                    .HasMaxLength(3)
                    .HasColumnName("airport_code")
                    .IsFixedLength(true)
                    .HasComment("Airport code");

                entity.Property(e => e.AirportName)
                    .HasColumnName("airport_name")
                    .HasComment("Airport name");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasComment("City");

                entity.Property(e => e.Coordinates)
                    .HasColumnName("coordinates")
                    .HasComment("Airport coordinates (longitude and latitude)");

                entity.Property(e => e.Timezone)
                    .HasColumnName("timezone")
                    .HasComment("Airport time zone");
            });

            modelBuilder.Entity<AirportsDatum>(entity =>
            {
                entity.HasKey(e => e.AirportCode)
                    .HasName("airports_data_pkey");

                entity.ToTable("airports_data", "bookings");

                entity.HasComment("Airports (internal data)");

                entity.Property(e => e.AirportCode)
                    .HasMaxLength(3)
                    .HasColumnName("airport_code")
                    .IsFixedLength(true)
                    .HasComment("Airport code");

                entity.Property(e => e.AirportName)
                    .IsRequired()
                    .HasColumnType("jsonb")
                    .HasColumnName("airport_name")
                    .HasComment("Airport name");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnType("jsonb")
                    .HasColumnName("city")
                    .HasComment("City");

                entity.Property(e => e.Coordinates)
                    .HasColumnName("coordinates")
                    .HasComment("Airport coordinates (longitude and latitude)");

                entity.Property(e => e.Timezone)
                    .IsRequired()
                    .HasColumnName("timezone")
                    .HasComment("Airport time zone");
            });

            modelBuilder.Entity<BoardingPass>(entity =>
            {
                entity.HasKey(e => new { e.TicketNo, e.FlightId })
                    .HasName("boarding_passes_pkey");

                entity.ToTable("boarding_passes", "bookings");

                entity.HasComment("Boarding passes");

                entity.HasIndex(e => new { e.FlightId, e.BoardingNo }, "boarding_passes_flight_id_boarding_no_key")
                    .IsUnique();

                entity.HasIndex(e => new { e.FlightId, e.SeatNo }, "boarding_passes_flight_id_seat_no_key")
                    .IsUnique();

                entity.Property(e => e.TicketNo)
                    .HasMaxLength(13)
                    .HasColumnName("ticket_no")
                    .IsFixedLength(true)
                    .HasComment("Ticket number");

                entity.Property(e => e.FlightId)
                    .HasColumnName("flight_id")
                    .HasComment("Flight ID");

                entity.Property(e => e.BoardingNo)
                    .HasColumnName("boarding_no")
                    .HasComment("Boarding pass number");

                entity.Property(e => e.SeatNo)
                    .IsRequired()
                    .HasMaxLength(4)
                    .HasColumnName("seat_no")
                    .HasComment("Seat number");

                entity.HasOne(d => d.TicketFlight)
                    .WithOne(p => p.BoardingPass)
                    .HasForeignKey<BoardingPass>(d => new { d.TicketNo, d.FlightId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("boarding_passes_ticket_no_fkey");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.BookRef)
                    .HasName("bookings_pkey");

                entity.ToTable("bookings", "bookings");

                entity.HasComment("Bookings");

                entity.Property(e => e.BookRef)
                    .HasMaxLength(6)
                    .HasColumnName("book_ref")
                    .IsFixedLength(true)
                    .HasComment("Booking number");

                entity.Property(e => e.BookDate)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("book_date")
                    .HasComment("Booking date");

                entity.Property(e => e.TotalAmount)
                    .HasPrecision(10, 2)
                    .HasColumnName("total_amount")
                    .HasComment("Total booking cost");
            });

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.ToTable("flights", "bookings");

                entity.HasComment("Flights");

                entity.HasIndex(e => new { e.FlightNo, e.ScheduledDeparture }, "flights_flight_no_scheduled_departure_key")
                    .IsUnique();

                entity.Property(e => e.FlightId)
                    .HasColumnName("flight_id")
                    .HasDefaultValueSql("nextval('flights_flight_id_seq'::regclass)")
                    .HasComment("Flight ID");

                entity.Property(e => e.ActualArrival)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("actual_arrival")
                    .HasComment("Actual arrival time");

                entity.Property(e => e.ActualDeparture)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("actual_departure")
                    .HasComment("Actual departure time");

                entity.Property(e => e.AircraftCode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("aircraft_code")
                    .IsFixedLength(true)
                    .HasComment("Aircraft code, IATA");

                entity.Property(e => e.ArrivalAirport)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("arrival_airport")
                    .IsFixedLength(true)
                    .HasComment("Airport of arrival");

                entity.Property(e => e.DepartureAirport)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("departure_airport")
                    .IsFixedLength(true)
                    .HasComment("Airport of departure");

                entity.Property(e => e.FlightNo)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("flight_no")
                    .IsFixedLength(true)
                    .HasComment("Flight number");

                entity.Property(e => e.ScheduledArrival)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("scheduled_arrival")
                    .HasComment("Scheduled arrival time");

                entity.Property(e => e.ScheduledDeparture)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("scheduled_departure")
                    .HasComment("Scheduled departure time");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("status")
                    .HasComment("Flight status");

                entity.HasOne(d => d.AircraftCodeNavigation)
                    .WithMany(p => p.Flights)
                    .HasForeignKey(d => d.AircraftCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("flights_aircraft_code_fkey");

                entity.HasOne(d => d.ArrivalAirportNavigation)
                    .WithMany(p => p.FlightArrivalAirportNavigations)
                    .HasForeignKey(d => d.ArrivalAirport)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("flights_arrival_airport_fkey");

                entity.HasOne(d => d.DepartureAirportNavigation)
                    .WithMany(p => p.FlightDepartureAirportNavigations)
                    .HasForeignKey(d => d.DepartureAirport)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("flights_departure_airport_fkey");
            });

            modelBuilder.Entity<FlightsV>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("flights_v", "bookings");

                entity.HasComment("Flights (extended)");

                entity.Property(e => e.ActualArrival)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("actual_arrival")
                    .HasComment("Actual arrival time");

                entity.Property(e => e.ActualArrivalLocal)
                    .HasColumnName("actual_arrival_local")
                    .HasComment("Actual arrival time, local time at the point of destination");

                entity.Property(e => e.ActualDeparture)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("actual_departure")
                    .HasComment("Actual departure time");

                entity.Property(e => e.ActualDepartureLocal)
                    .HasColumnName("actual_departure_local")
                    .HasComment("Actual departure time, local time at the point of departure");

                entity.Property(e => e.ActualDuration)
                    .HasColumnName("actual_duration")
                    .HasComment("Actual flight duration");

                entity.Property(e => e.AircraftCode)
                    .HasMaxLength(3)
                    .HasColumnName("aircraft_code")
                    .IsFixedLength(true)
                    .HasComment("Aircraft code, IATA");

                entity.Property(e => e.ArrivalAirport)
                    .HasMaxLength(3)
                    .HasColumnName("arrival_airport")
                    .IsFixedLength(true)
                    .HasComment("Arrival airport code");

                entity.Property(e => e.ArrivalAirportName)
                    .HasColumnName("arrival_airport_name")
                    .HasComment("Arrival airport name");

                entity.Property(e => e.ArrivalCity)
                    .HasColumnName("arrival_city")
                    .HasComment("City of arrival");

                entity.Property(e => e.DepartureAirport)
                    .HasMaxLength(3)
                    .HasColumnName("departure_airport")
                    .IsFixedLength(true)
                    .HasComment("Deprature airport code");

                entity.Property(e => e.DepartureAirportName)
                    .HasColumnName("departure_airport_name")
                    .HasComment("Departure airport name");

                entity.Property(e => e.DepartureCity)
                    .HasColumnName("departure_city")
                    .HasComment("City of departure");

                entity.Property(e => e.FlightId)
                    .HasColumnName("flight_id")
                    .HasComment("Flight ID");

                entity.Property(e => e.FlightNo)
                    .HasMaxLength(6)
                    .HasColumnName("flight_no")
                    .IsFixedLength(true)
                    .HasComment("Flight number");

                entity.Property(e => e.ScheduledArrival)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("scheduled_arrival")
                    .HasComment("Scheduled arrival time");

                entity.Property(e => e.ScheduledArrivalLocal)
                    .HasColumnName("scheduled_arrival_local")
                    .HasComment("Scheduled arrival time, local time at the point of destination");

                entity.Property(e => e.ScheduledDeparture)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("scheduled_departure")
                    .HasComment("Scheduled departure time");

                entity.Property(e => e.ScheduledDepartureLocal)
                    .HasColumnName("scheduled_departure_local")
                    .HasComment("Scheduled departure time, local time at the point of departure");

                entity.Property(e => e.ScheduledDuration)
                    .HasColumnName("scheduled_duration")
                    .HasComment("Scheduled flight duration");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasColumnName("status")
                    .HasComment("Flight status");
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("routes", "bookings");

                entity.HasComment("Routes");

                entity.Property(e => e.AircraftCode)
                    .HasMaxLength(3)
                    .HasColumnName("aircraft_code")
                    .IsFixedLength(true)
                    .HasComment("Aircraft code, IATA");

                entity.Property(e => e.ArrivalAirport)
                    .HasMaxLength(3)
                    .HasColumnName("arrival_airport")
                    .IsFixedLength(true)
                    .HasComment("Code of airport of arrival");

                entity.Property(e => e.ArrivalAirportName)
                    .HasColumnName("arrival_airport_name")
                    .HasComment("Name of airport of arrival");

                entity.Property(e => e.ArrivalCity)
                    .HasColumnName("arrival_city")
                    .HasComment("City of arrival");

                entity.Property(e => e.DaysOfWeek)
                    .HasColumnName("days_of_week")
                    .HasComment("Days of week on which flights are scheduled");

                entity.Property(e => e.DepartureAirport)
                    .HasMaxLength(3)
                    .HasColumnName("departure_airport")
                    .IsFixedLength(true)
                    .HasComment("Code of airport of departure");

                entity.Property(e => e.DepartureAirportName)
                    .HasColumnName("departure_airport_name")
                    .HasComment("Name of airport of departure");

                entity.Property(e => e.DepartureCity)
                    .HasColumnName("departure_city")
                    .HasComment("City of departure");

                entity.Property(e => e.Duration)
                    .HasColumnName("duration")
                    .HasComment("Scheduled duration of flight");

                entity.Property(e => e.FlightNo)
                    .HasMaxLength(6)
                    .HasColumnName("flight_no")
                    .IsFixedLength(true)
                    .HasComment("Flight number");
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasKey(e => new { e.AircraftCode, e.SeatNo })
                    .HasName("seats_pkey");

                entity.ToTable("seats", "bookings");

                entity.HasComment("Seats");

                entity.Property(e => e.AircraftCode)
                    .HasMaxLength(3)
                    .HasColumnName("aircraft_code")
                    .IsFixedLength(true)
                    .HasComment("Aircraft code, IATA");

                entity.Property(e => e.SeatNo)
                    .HasMaxLength(4)
                    .HasColumnName("seat_no")
                    .HasComment("Seat number");

                entity.Property(e => e.FareConditions)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("fare_conditions")
                    .HasComment("Travel class");

                entity.HasOne(d => d.AircraftCodeNavigation)
                    .WithMany(p => p.Seats)
                    .HasForeignKey(d => d.AircraftCode)
                    .HasConstraintName("seats_aircraft_code_fkey");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.TicketNo)
                    .HasName("tickets_pkey");

                entity.ToTable("tickets", "bookings");

                entity.HasComment("Tickets");

                entity.Property(e => e.TicketNo)
                    .HasMaxLength(13)
                    .HasColumnName("ticket_no")
                    .IsFixedLength(true)
                    .HasComment("Ticket number");

                entity.Property(e => e.BookRef)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("book_ref")
                    .IsFixedLength(true)
                    .HasComment("Booking number");

                entity.Property(e => e.ContactData)
                    .HasColumnType("jsonb")
                    .HasColumnName("contact_data")
                    .HasComment("Passenger contact information");

                entity.Property(e => e.PassengerId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("passenger_id")
                    .HasComment("Passenger ID");

                entity.Property(e => e.PassengerName)
                    .IsRequired()
                    .HasColumnName("passenger_name")
                    .HasComment("Passenger name");

                entity.HasOne(d => d.BookRefNavigation)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.BookRef)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tickets_book_ref_fkey");
            });

            modelBuilder.Entity<TicketFlight>(entity =>
            {
                entity.HasKey(e => new { e.TicketNo, e.FlightId })
                    .HasName("ticket_flights_pkey");

                entity.ToTable("ticket_flights", "bookings");

                entity.HasComment("Flight segment");

                entity.Property(e => e.TicketNo)
                    .HasMaxLength(13)
                    .HasColumnName("ticket_no")
                    .IsFixedLength(true)
                    .HasComment("Ticket number");

                entity.Property(e => e.FlightId)
                    .HasColumnName("flight_id")
                    .HasComment("Flight ID");

                entity.Property(e => e.Amount)
                    .HasPrecision(10, 2)
                    .HasColumnName("amount")
                    .HasComment("Travel cost");

                entity.Property(e => e.FareConditions)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("fare_conditions")
                    .HasComment("Travel class");

                entity.HasOne(d => d.Flight)
                    .WithMany(p => p.TicketFlights)
                    .HasForeignKey(d => d.FlightId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ticket_flights_flight_id_fkey");

                entity.HasOne(d => d.TicketNoNavigation)
                    .WithMany(p => p.TicketFlights)
                    .HasForeignKey(d => d.TicketNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ticket_flights_ticket_no_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
