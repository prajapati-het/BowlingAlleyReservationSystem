using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace BowlingAlley.Services
{
    public class SlotCleanerService : IHostedService
    {
        private readonly string _connectionString;

        public SlotCleanerService(IConfiguration configuration)
        {
            // Retrieve connection string from appsettings.json
            _connectionString = configuration.GetConnectionString("BowlingAlleyDBConnectionString");
        }

        // Method called when the application starts
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Run the deletion logic immediately when the application starts
            await DeleteExpiredReservationsAsync();
        }

        // Method called when the application stops
        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Optionally, you can add any clean-up logic if necessary when the application stops
            return Task.CompletedTask;
        }

        // Method to delete expired reservations
        private async Task DeleteExpiredReservationsAsync()
        {
            // SQL query to delete expired reservations
            string query = @"
                DELETE FROM Reservations
                WHERE ReservationId IN (
                    SELECT r.ReservationId
                    FROM Reservations r
                    INNER JOIN BookingSlots bs ON r.SlotId = bs.SlotId
                    WHERE GETDATE() > bs.SlotEndTime
                );";

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Execute the SQL query
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions
                Console.WriteLine($"Error while deleting expired reservations: {ex.Message}");
            }
        }
    }
}
