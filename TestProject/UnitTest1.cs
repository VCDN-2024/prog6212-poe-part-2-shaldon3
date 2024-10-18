using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProgTest.Controllers;
using ProgTest.Data;
using ProgTest.Models;

namespace TestProject
{
    [TestFixture]
    public class ClaimsControllerTests
    {
        private ClaimsController _controller;
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Create a mock in-memory database with a unique name for each test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Ensures a unique database for each test
                .Options;

            // Create the context
            _context = new ApplicationDbContext(options);

            // Seed the database with test data
            _context.Claims.AddRange(new List<Claim>
            {
                new Claim { ClaimId = 1, LecturerName = "John Doe", HoursWorked = 10, HourlyRate = 20, ClaimDate = DateTime.Now, IsSubmitted = false },
                new Claim { ClaimId = 2, LecturerName = "Jane Smith", HoursWorked = 15, HourlyRate = 30, ClaimDate = DateTime.Now, IsSubmitted = true }
            });
            _context.SaveChanges();

            // Create the controller with the mocked context
            _controller = new ClaimsController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose of the context
            _context?.Dispose();

            // Dispose of the controller if it implements IDisposable
            (_controller as IDisposable)?.Dispose(); // This line ensures proper disposal
            _controller = null; // Optional, helps with memory management
        }

        [Test]
        public async Task Index_ReturnsViewResult_WithListOfClaims()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = result as ViewResult; // Use as to cast
            Assert.That(viewResult, Is.Not.Null); // Ensure the result is not null

            var model = viewResult.Model as List<Claim>; // Cast the model
            Assert.That(model, Is.Not.Null); // Ensure the model is not null
            Assert.That(model.Count, Is.EqualTo(2)); // Verify that the number of claims is as expected
        }

        [Test]
        public async Task Create_ValidClaim_ReturnsRedirectToIndexAndAddsClaim()
        {
            // Arrange
            var newClaim = new Claim
            {
                ClaimId = 3,
                LecturerName = "Alice Johnson",
                HoursWorked = 12,
                HourlyRate = 25,
                ClaimDate = DateTime.Now,
                IsSubmitted = false
            };

            // Act
            var result = await _controller.Create(newClaim); // Assuming Create method accepts Claim as parameter

            // Assert
            var redirectResult = result as RedirectToActionResult; // Check for redirect result
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index")); // Ensure it redirects to Index

            // Verify the claim was added
            var claimsInDb = await _context.Claims.ToListAsync();
            Assert.That(claimsInDb.Count, Is.EqualTo(3)); // Ensure there are now 3 claims
            Assert.That(claimsInDb.Last().LecturerName, Is.EqualTo(newClaim.LecturerName)); // Check the last claim is the new one
        }
    }
}
