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
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients;
using Iwentys.Endpoint.Sdk.ControllerClients.Study;
using Iwentys.Features.Achievements.Models;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.StudentFeature.Models;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Students
{
    public partial class StudentProfilePage : ComponentBase
    {

        private StudentPartialProfileDto _studentFullProfile;
        private List<AchievementDto> _achievements;
        private List<CodingActivityInfoResponse> _codingActivityInfo;
        private StudentActivityInfoDto _studentActivity;

        private LineConfig _githubChartConfig;
        private ChartJsLineChart _githubChart;

        private PieConfig _studyChartConfig;
        private ChartJsPieChart _studyChart;

        protected override async Task OnInitializedAsync()
        {
            var httpClient = await Http.TrySetHeader(LocalStorage);
            var studentControllerClient = new StudentControllerClient(httpClient);
            var githubControllerClient = new GithubControllerClient(httpClient);
            var studyLeaderboardControllerClient = new StudyLeaderboardControllerClient(httpClient);

            if (StudentId is null)
            {

                _studentFullProfile = await studentControllerClient.GetSelf();
            }
            else
            {
                _studentFullProfile = await studentControllerClient.Get(StudentId.Value);
            }

            _codingActivityInfo = await githubControllerClient.Get(_studentFullProfile.Id);
            if (_codingActivityInfo is not null)
                InitGithubChart();

            _studentActivity = await studyLeaderboardControllerClient.GetStudentActivity(_studentFullProfile.Id);
            if (_studentActivity is not null)
                InitStudyChart();

            _achievements = await new AchievementControllerClient(httpClient).GetForStudent(_studentFullProfile.Id);
        }

        private void InitGithubChart()
        {
            IEnumerable<Int32Wrapper> elements = _codingActivityInfo?.Select(a => new Int32Wrapper(a.Activity))
                                                 ?? new List<Int32Wrapper>();

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

            //TODO: add real month
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
