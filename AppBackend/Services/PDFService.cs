using System.ComponentModel;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AppBackend.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace  AppBackend.Services
{
    public class PDFService : IPDFService
    {
        public byte[] GenerateTicketPdf(TicketBooking booking,List<SeatWithFlightDto> seatWithFlights){

 var document = Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A5.Landscape());
            page.Margin(25);
            page.DefaultTextStyle(x => x.FontSize(11));

            page.Content().Column(column =>
            {
                column.Spacing(10);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Text($"Flight Ticket - {booking.ConfirmationCode}")
                        .FontSize(16).Bold();

                    row.ConstantItem(200).AlignRight().Text($"Passenger: {booking.FirstName} {booking.LastName}")
                        .FontSize(12);
                });

                column.Item().LineHorizontal(1);
                column.Item().Text("WPI Flights  |  Phone: +1 (888) 888 8888  |  Email: flightwithus@gmail.com").FontSize(10);
                column.Item().LineHorizontal(1);

                var grouped = seatWithFlights
                    .GroupBy(x => x.Direction?.ToLower())
                    .OrderBy(g => g.Key == "inbound" ? 0 : 1);

                foreach (var group in grouped)
                {
                    var directionLabel = group.Key == "inbound" ? "Departure Flight" : "Return Flight";
                    column.Item().Text(directionLabel).Bold().FontSize(12);

                    var seatGroups = group
                        .GroupBy(x => new { x.SeatNumber, x.FlightSource, x.Flight })
                        .Select(g => g.First());

                    foreach (var item in seatGroups)
                    {
                        var barcodeBytes = GenerateBarCode.GenerateBarcode($"{booking.ConfirmationCode}:{item.SeatNumber}");

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Spacing(2);
                                col.Item().Row(seatRow =>
                            {
                                seatRow.RelativeItem().Text($"Seat: {item.SeatNumber}").Bold();
                                seatRow.ConstantItem(100).AlignRight().Text($"Gate: {item.Gate}").FontColor(Colors.Grey.Darken2);
                                    });

                                if (item.FlightSource == "Delta" && item.Flight is Deltas delta)
                                {
                                    col.Item().Text($"Delta Flight: {delta.FlightNumber}");
                                    col.Item().Text($"From {delta.DepartAirport} to {delta.ArriveAirport}");
                                   col.Item().Row(timeRow =>
                                {
                                    timeRow.RelativeItem().Text($"Departs: {delta.DepartDateTime:MMM dd, yyyy - hh:mm tt}");
                                    timeRow.ConstantItem(220).AlignRight().Text($"Arrives: {delta.ArriveDateTime:MMM dd, yyyy - hh:mm tt}");
                                });

                                    col.Item().Text($"Duration: {(delta.ArriveDateTime - delta.DepartDateTime)}");

                                }
                                else if (item.FlightSource == "Southwest" && item.Flight is SouthWests sw)
                                {
                                    col.Item().Text($"Southwest Flight: {sw.FlightNumber}");
                                    col.Item().Text($"From {sw.DepartAirport} to {sw.ArriveAirport}");
                                    col.Item().Row(timeRow =>
                                {
                                  timeRow.RelativeItem().Text($"Departs: {sw.DepartDateTime:MMM dd, yyyy - hh:mm tt}");
                                  timeRow.ConstantItem(220).AlignRight().Text($"Arrives: {sw.ArriveDateTime:MMM dd, yyyy - hh:mm tt}");
                                 });

                                    col.Item().Text($"Duration: {(sw.ArriveDateTime - sw.DepartDateTime)}");

                                }
                            });

                            row.ConstantItem(140).Image(barcodeBytes);
                        });

                        column.Item().LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);
                    }
                }

                column.Item().AlignCenter().Text("Thank you for choosing our airline.").Italic().FontColor(Colors.Grey.Darken2);
                column.Item().AlignCenter().Text("With us, the sky is the limit.").Italic().FontColor(Colors.Grey.Darken2);
            });
        });
    });

    return document.GeneratePdf();




    }
    }
}


