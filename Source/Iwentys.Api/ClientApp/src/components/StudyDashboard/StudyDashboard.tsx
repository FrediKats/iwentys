import React from 'react';
import {Pie} from "@ant-design/charts";
import {ISubjectActivityInfo} from "../../redux/typings";
import './StudyDashboard.scss'

export interface IStudyDashboardProps {
    subjectActivityInfo: ISubjectActivityInfo[];
}
export const StudyDashboard: React.FC<IStudyDashboardProps> = ({subjectActivityInfo}) => {

    const data = [{}];

    // is necessary for the config, because the DataItem object is needed, and not any other
    subjectActivityInfo.forEach(item => data.push({type: item.subjectTitle, value: item.points}));

    const config = {
        forceFit: true,
        title: {
            visible: true,
            text: 'Учеба',
        },
        description: {
            visible: false,
            text: '',
        },
        radius: 0.8,
        data,
        angleField: 'value',
        colorField: 'type',
        label: {
            visible: false,
            type: 'inner',
        },
    };

    return (
        <section className={'StudyDashboard'}>
            <Pie {...config}/>
        </section>
        )
}

