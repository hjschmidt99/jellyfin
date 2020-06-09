using System;
using System.Globalization;
using Jellyfin.Api.Constants;
using MediaBrowser.Model.Activity;
using MediaBrowser.Model.Querying;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jellyfin.Api.Controllers.System
{
    /// <summary>
    /// Activity log controller.
    /// </summary>
    [Route("/System/ActivityLog/Entries")]
    [Authorize(Policy = Policies.RequiresElevation)]
    public class ActivityLogController : BaseJellyfinApiController
    {
        private readonly IActivityManager _activityManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityLogController"/> class.
        /// </summary>
        /// <param name="activityManager">Instance of <see cref="IActivityManager"/> interface.</param>
        public ActivityLogController(IActivityManager activityManager)
        {
            _activityManager = activityManager;
        }

        /// <summary>
        /// Gets activity log entries.
        /// </summary>
        /// <param name="startIndex">Optional. The record index to start at. All items with a lower index will be dropped from the results.</param>
        /// <param name="limit">Optional. The maximum number of records to return.</param>
        /// <param name="minDate">Optional. The minimum date. Format = ISO.</param>
        /// <param name="hasUserId">Optional. Only returns activities that have a user associated.</param>
        /// <response code="200">Activity log returned.</response>
        /// <returns>A <see cref="QueryResult{ActivityLogEntry}"/> containing the log entries.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<QueryResult<ActivityLogEntry>> GetLogEntries(
            [FromQuery] int? startIndex,
            [FromQuery] int? limit,
            [FromQuery] string minDate,
            bool? hasUserId)
        {
            DateTime? startDate = string.IsNullOrWhiteSpace(minDate) ?
                (DateTime?)null :
                DateTime.Parse(minDate, null, DateTimeStyles.RoundtripKind).ToUniversalTime();

            return _activityManager.GetActivityLogEntries(startDate, hasUserId, startIndex, limit);
        }
    }
}
