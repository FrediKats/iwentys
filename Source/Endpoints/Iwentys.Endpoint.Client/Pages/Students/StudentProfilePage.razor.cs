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
using Iwentys.Endpoint.Sdk.ControllerClients.Study;
using Iwentys.Models.Transferable.Students;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Students
{
    public partial class StudentProfilePage : ComponentBase
    {

        private StudentFullProfileDto _studentFullProfile;

        private LineConfig _githubChartConfig;
        private ChartJsLineChart _githubChart;

        private PieConfig _studyChartConfig;
        private ChartJsPieChart _studyChart;

        protected override async Task OnInitializedAsync()
        {
            var studentControllerClient = new StudentControllerClient(await Http.TrySetHeader(LocalStorage));

            if (StudentId is null)
            {

                _studentFullProfile = await studentControllerClient.GetSelf();
            }
            else
            {
                _studentFullProfile = await studentControllerClient.Get(StudentId.Value);
            }

            if (_studentFullProfile.CodingActivityInfo is not null)
                InitGithubChart();

            if (_studentFullProfile.SubjectActivityInfo is not null)
                InitStudyChart();
        }

        private void InitGithubChart()
        {
            IEnumerable<Int32Wrapper> elements = _studentFullProfile?.CodingActivityInfo?.Select(a => new Int32Wrapper(a.Activity))
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

            if (_studentFullProfile?.SubjectActivityInfo is not null)
            {
                pieSet.Data.AddRange(_studentFullProfile.SubjectActivityInfo.Select(sa => sa.Points));
                _studyChartConfig.Data.Labels.AddRange(_studentFullProfile.SubjectActivityInfo.Select(sa => sa.SubjectTitle));
            }

            _studyChartConfig.Data.Datasets.Add(pieSet);
        }
    }
}
