import React from 'react';
import './Badge.scss';

export interface IBadgeProps {
    content: string[];
}
export const Badge: React.FC<IBadgeProps> = ({content}) => {

    return (
        <>
            {content.map(item => (
                <span key={item} className={'badge badge-pill'}>
                    {item}
                </span>
            ))}
        </>
    );
}
