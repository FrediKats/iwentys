using System.Drawing;
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

namespace Iwentys.WebClient.Content
{
    public partial class StudentProfilePage
    {
        private bool _isSelf;
        private StudentInfoDto _studentFullProfile;
        private ICollection<AchievementInfoDto> _achievements;
        private ICollection<CodingActivityInfoResponse> _codingActivityInfo;
        //TODO: fix this
        //private StudentActivityInfoDto _studentActivity;
        private CourseLeaderboardRow _leaderboardRow;

        private LineConfig _githubChartConfig;
        private ChartJsLineChart _githubChart;

        private PieConfig _studyChartConfig;
        private ChartJsPieChart _studyChart;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _logger.LogTrace("Load student profile");

            if (StudentId is null)
            {
                _studentFullProfile = await _studentClient.GetSelfAsync();
                _isSelf = true;
            }
            else
            {
                _studentFullProfile = await _studentClient.GetByIdAsync(StudentId.Value);
                _isSelf = false;
            }

            _codingActivityInfo = await _githubClient.GetByStudentIdAsync(_studentFullProfile.Id);
            if (_codingActivityInfo is not null)
                InitGithubChart();

            //TODO: we lose this method while refac. Try to find this in history
            //_studentActivity = await LeaderboardClient. ActivityAsync(_studentFullProfile.Id);
            //if (_studentActivity is not null)
            //    InitStudyChart();

            _achievements = await _achievementClient.GetByStudentIdAsync(_studentFullProfile.Id);
            try
            {
                _leaderboardRow = await _leaderboardClient.StudentPositionAsync(_studentFullProfile.Id);
            }
            catch (Exception e)
            {
                //TODO: remove this hack. Implement logic for handling 404 or null value
                _logger.Log(LogLevel.Error, e, "Failed to get student rating position.");
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
            _githubChartConfig.Data.Labels = Enumerable.Repeat("", _codingActivityInfo.Count).ToList();
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

            var colors = new[]
            {
                "#003f5c",
                "#2f4b7c",
                "#665191",
                "#a05195",
                "#d45087",
                "#f95d6a",
                "#ff7c43",
                "#ffa600",
            };

            var pieSet = new PieDataset
            {
                BackgroundColor = colors,
                BorderWidth = 0,
                HoverBackgroundColor = "#ffa600",
                HoverBorderWidth = 1,
                BorderColor = "#ffffff",
            };

            //if (_studentActivity is not null)
            //{
            //    var data = _studentActivity.Activity.OrderBy(sa => sa.Points).ToList();
            //    pieSet.Data.AddRange(data.Select(sa => sa.Points));
            //    _studyChartConfig.Data.Labels.AddRange(data.Select(sa => sa.SubjectTitle));
            //}

            _studyChartConfig.Data.Datasets.Add(pieSet);
        }
    }
}
