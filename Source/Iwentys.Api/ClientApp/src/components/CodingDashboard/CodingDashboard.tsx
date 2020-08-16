import React from 'react';
import {Area} from '@ant-design/charts';
import {ICodingActivityInfo} from "../../redux/typings";


export interface ICodingDashboardProps {
    codingActivityInfo: ICodingActivityInfo[];
}
export const CodingDashboard: React.FC<ICodingDashboardProps> = ({codingActivityInfo}) => {

    const data = [{}];

    // is necessary for the config, because the DataItem object is needed, and not any other
    for (let item of codingActivityInfo) {
        data.push({
            month: item.month,
            activity: item.activity
        });
    }

    const config = {
        data,
        title: {
            visible: true,
            text: 'Кодинг',
        },
        xField: 'month',
        yField: 'activity',
        point: {
            visible: true,
            size: 5,
            shape: 'diamond',
            style: {
                fill: 'white',
                stroke: '#2593fc',
                lineWidth: 2,
            },
        },
    };
    return <Area {...config} />;
}
