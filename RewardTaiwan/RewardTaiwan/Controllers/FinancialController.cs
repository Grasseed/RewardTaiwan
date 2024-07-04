using Microsoft.AspNetCore.Mvc;
using RewardTaiwan.Services;
using RewardTaiwan.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RewardTaiwan.API
{
    [ApiController]
    [Route("[controller]")]
    public class FinancialController : ControllerBase
    {
        private readonly IDapper _dapperService;

        public FinancialController(IDapper dapperService)
        {
            _dapperService = dapperService;
        }

        [HttpGet("banks")]
        public async Task<IActionResult> GetAllBanksAsync()
        {
            try
            {
                var banks_Zh = await _dapperService.GetAllBankNamesAsync();
                return Ok(banks_Zh);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("credit_cards")]
        public async Task<IActionResult> GetCreditCardsWithBanksAsync()
        {
            try
            {
                var creditCardsWithBanks = await _dapperService.GetAllCreditCardsWithBanksAsync();
                return Ok(creditCardsWithBanks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
