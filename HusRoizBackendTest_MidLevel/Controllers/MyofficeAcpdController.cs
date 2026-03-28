using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using HusRoizBackendTest_MidLevel.Models;

namespace HusRoizBackendTest_MidLevel.Controllers
{
    [ApiController]
    [Route("api/myofficeacpd")]
    public class MyofficeAcpdController : ControllerBase
    {
        private readonly string _connectString;

        public MyofficeAcpdController(IConfiguration config)
        {
            _connectString = config.GetConnectionString("DefaultConnection") ?? "";
        }

        /// <summary>
        /// 1. 查詢所有資料 (GET)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                using var db = new SqlConnection(_connectString);
                var sql = "SELECT * FROM MyOffice_ACPD";
                var result = await db.QueryAsync(sql);
                return Ok(result);
            }
            catch (Exception ex)
            {
                await LogErrorAsync(Guid.NewGuid(), "GetAll", ex.Message);
                return StatusCode(500, "內部伺服器錯誤");
            }
        }

        /// <summary>
        /// 1.1 查詢單筆資料 (GET id)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                using var db = new SqlConnection(_connectString);
                var sql = "SELECT * FROM MyOffice_ACPD WHERE ACPD_SID = @Id";
                var result = await db.QueryFirstOrDefaultAsync(sql, new { Id = id });
                
                if (result == null)
                    return NotFound(new { message = "資源不存在" });
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                await LogErrorAsync(Guid.NewGuid(), "GetById", ex.Message);
                return StatusCode(500, "內部伺服器錯誤");
            }
        }

        /// <summary>
        /// 2. 新增資料 (POST)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MyofficeAcpdDto data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using var db = new SqlConnection(_connectString);
            Guid groupID = Guid.NewGuid();

            try
            {
                // A. 呼叫 NEWSID 產生主鍵
                var p = new DynamicParameters();
                p.Add("@TableName", "MyOffice_ACPD");
                p.Add("@ReturnSID", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                await db.ExecuteAsync("NEWSID", p, commandType: CommandType.StoredProcedure);
                string newSid = p.Get<string>("@ReturnSID");

                // B. 執行新增
                var sql = @"INSERT INTO MyOffice_ACPD 
                            (acpd_sid, acpd_cname, acpd_ename, acpd_sname, acpd_email, acpd_status, acpd_stop, acpd_stopMemo, 
                             acpd_LoginID, acpd_LoginPW, acpd_memo, acpd_nowdatetime, appd_nowid) 
                            VALUES 
                            (@Sid, @Cname, @Ename, @Sname, @Email, @Status, @Stop, @StopMemo, 
                             @LoginID, @LoginPW, @Memo, GETDATE(), @AppdNowid)";
                
                await db.ExecuteAsync(sql, new
                {
                    Sid = newSid,
                    Cname = data.AcpdCname,
                    Ename = data.AcpdEname,
                    Sname = data.AcpdSname,
                    Email = data.AcpdEmail,
                    Status = data.AcpdStatus ?? 0,
                    Stop = data.AcpdStop ?? false,
                    StopMemo = data.AcpdStopMemo,
                    LoginID = data.AcpdLoginID,
                    LoginPW = data.AcpdLoginPW,
                    Memo = data.AcpdMemo,
                    AppdNowid = data.AppdNowid
                });

                return StatusCode(201, new { id = newSid });
            }
            catch (Exception ex)
            {
                await LogErrorAsync(groupID, "Create", ex.Message);
                return StatusCode(500, "內部伺服器錯誤");
            }
        }

        /// <summary>
        /// 3. 更新資料 (PUT)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] MyofficeAcpdDto data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using var db = new SqlConnection(_connectString);
            Guid groupID = Guid.NewGuid();

            try
            {
                // 是否存在
                var checkSql = "SELECT COUNT(1) FROM MyOffice_ACPD WHERE acpd_sid = @Id";
                var exists = await db.ExecuteScalarAsync<int>(checkSql, new { Id = id });
                if (exists == 0) return NotFound(new { message = "資源不存在" });

                var sql = @"UPDATE MyOffice_ACPD 
                            SET acpd_cname = @Cname, 
                                acpd_ename = @Ename, 
                                acpd_sname = @Sname, 
                                acpd_email = @Email, 
                                acpd_status = @Status, 
                                acpd_stop = @Stop, 
                                acpd_stopMemo = @StopMemo, 
                                acpd_LoginID = @LoginID, 
                                acpd_LoginPW = @LoginPW, 
                                acpd_memo = @Memo,
                                acpd_upddatetitme = GETDATE(),
                                acpd_updid = @Updid
                            WHERE acpd_sid = @Id";

                await db.ExecuteAsync(sql, new
                {
                    Id = id,
                    Cname = data.AcpdCname,
                    Ename = data.AcpdEname,
                    Sname = data.AcpdSname,
                    Email = data.AcpdEmail,
                    Status = data.AcpdStatus ?? 0,
                    Stop = data.AcpdStop ?? false,
                    StopMemo = data.AcpdStopMemo,
                    LoginID = data.AcpdLoginID,
                    LoginPW = data.AcpdLoginPW,
                    Memo = data.AcpdMemo,
                    Updid = data.AcpdUpdid
                });

                return Ok(new { message = "更新成功" });
            }
            catch (Exception ex)
            {
                await LogErrorAsync(groupID, "Update", ex.Message);
                return StatusCode(500, "內部伺服器錯誤");
            }
        }

        /// <summary>
        /// 4. 刪除資料 (DELETE)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using var db = new SqlConnection(_connectString);
            Guid groupID = Guid.NewGuid();

            try
            {
                var checkSql = "SELECT COUNT(1) FROM MyOffice_ACPD WHERE acpd_sid = @Id";
                var exists = await db.ExecuteScalarAsync<int>(checkSql, new { Id = id });
                if (exists == 0) return NotFound(new { message = "資源不存在" });

                var sql = "DELETE FROM MyOffice_ACPD WHERE acpd_sid = @Id";
                await db.ExecuteAsync(sql, new { Id = id });

                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                await LogErrorAsync(groupID, "Delete", ex.Message);
                return StatusCode(500, "內部伺服器錯誤");
            }
        }

        private async Task LogErrorAsync(Guid groupID, string actionName, string errorMessage)
        {
            try
            {
                using var db = new SqlConnection(_connectString);
                await db.ExecuteAsync("usp_AddLog", new
                {
                    _InBox_ReadID = 0,
                    _InBox_SPNAME = $"API_{actionName}",
                    _InBox_GroupID = groupID,
                    _InBox_ExProgram = $"{actionName}Action",
                    _InBox_ActionJSON = errorMessage
                }, commandType: CommandType.StoredProcedure);
            }
            catch { /* 如果寫入Log失敗，不再拋出異常避免無窮迴圈 */ }
        }
    }
}