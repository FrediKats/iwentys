using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ChartJs.Blazor.ChartJS.Common.Axes;
using ChartJs.Blazor.ChartJS.Common.Enums;
using ChartJs.Blazor.ChartJS.Common.Properties;
using ChartJs.Blazor.ChartJS.Common.Wrappers;
using ChartJs.Blazor.ChartJS.LineChart;
using ChartJs.Blazor.ChartJS.PieChart;
using ChartJs.Blazor.Charts;
using ChartJs.Blazor.Util;
using Iwentys.Sdk;
using Microsoft.Extensions.Logging;

namespace Iwentys.Endpoint.Client.Pages.Students
{
    public partial class StudentProfilePage
    {
        private bool _isSelf;
        private StudentInfoDto _studentFullProfile;
        private ICollection<AchievementInfoDto> _achievements;
        private ICollection<CodingActivityInfoResponse> _codingActivityInfo;
        private StudentActivityInfoDto _studentActivity;
        private CourseLeaderboardRow _leaderboardRow;

        private LineConfig _githubChartConfig;
        private ChartJsLineChart _githubChart;

        private PieConfig _studyChartConfig;
        private ChartJsPieChart _studyChart;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Logger.LogTrace("Load student profile");

            if (StudentId is null)
            {
                _studentFullProfile = await ClientHolder.ApiStudentSelfAsync();
                _isSelf = true;
            }
            else
            {
                _studentFullProfile = await ClientHolder.ApiStudentProfileGetAsync(StudentId.Value);
                _isSelf = false;
            }

            _codingActivityInfo = await ClientHolder.ApiGithubStudentAsync(_studentFullProfile.Id);
            if (_codingActivityInfo is not null)
                InitGithubChart();

            _studentActivity = await ClientHolder.ApiLeaderboardActivityAsync(_studentFullProfile.Id);
            if (_studentActivity is not null)
                InitStudyChart();

            _achievements = await ClientHolder.ApiAchievementsStudentsAsync(_studentFullProfile.Id);
            try
            {
                _leaderboardRow = await ClientHolder.ApiLeaderboardStudentPositionAsync(_studentFullProfile.Id);
            }
            catch (Exception e)
            {
                //TODO: remove this hack. Implement logic for handling 404 or null value
                Logger.Log(LogLevel.Error, e, "Failed to get student rating position.");
            }
        }

        private void InitGithubChart()
        {
            IEnumerable<Int32Wrapper> elements = _codingActivityInfo.Select(a => new Int32Wrapper(a.Activity));

            var lineDataset = new LineDataset<Int32Wrapper>(elements)
            {
                Label = "Github",
                BackgroundColor = ColorUtil.FromDrawingColor(Color.PaleVioletRed),
                PointBackgroundColor = ColorUtil.FromDrawingColor(Color.BlueViolet),
                Fill = true
            };

            _githubChartConfig = new LineConfig
            {
                Options = new LineOptions
                {
                    Responsive = true,
                    Tooltips = new Tooltips
                    {
                        Mode = InteractionMode.Nearest,
                        Intersect = true
                    },
                    Scales = new Scales
                    {
                        xAxes = new List<CartesianAxis>
                    {
                        new CategoryAxis
                        {
                            ScaleLabel = new ScaleLabel
                            {
                                LabelString = "Month"
                            }
                        }
                    },
                        yAxes = new List<CartesianAxis>
                    {
                        new LinearCartesianAxis
                        {
                            ScaleLabel = new ScaleLabel
                            {
                                LabelString = "Value"
                            }
                        }
                    }
                    },
                }
            };

            _githubChartConfig.Data.Labels = _codingActivityInfo.Select(a => a.Month).ToList();
            _githubChartConfig.Data.Labels = Enumerable.Repeat("", 12).ToList();
            _githubChartConfig.Data.Datasets.Add(lineDataset);
        }

        private void InitStudyChart()
        {
            _studyChartConfig = new PieConfig
            {
                Options = new PieOptions
                {
                    Responsive = true,
                    Animation = new ArcAnimation
                    {
                        AnimateRotate = true,
                        AnimateScale = true
                    }
                }
            };

            var pieSet = new PieDataset
            {
                BackgroundColor = new[] { ColorUtil.RandomColorString(), ColorUtil.RandomColorString(), ColorUtil.RandomColorString(), ColorUtil.RandomColorString() },
                BorderWidth = 0,
                HoverBackgroundColor = ColorUtil.RandomColorString(),
                HoverBorderColor = ColorUtil.RandomColorString(),
                HoverBorderWidth = 1,
                BorderColor = "#ffffff",
            };

            if (_studentActivity is not null)
            {
                pieSet.Data.AddRange(_studentActivity.Activity.Select(sa => sa.Points));
                _studyChartConfig.Data.Labels.AddRange(_studentActivity.Activity.Select(sa => sa.SubjectTitle));
            }

            _studyChartConfig.Data.Datasets.Add(pieSet);
        }
    }
}
