﻿module.exports = {
    async headers() {
        return [
            {
                source: '/:path*',
                headers: [
                    { key: 'Access-Control-Allow-Credentials', value: 'true' },
                    { key: 'Access-Control-Allow-Origin', value: '*' },
                    { key: 'Access-Control-Allow-Methods', value: 'GET,OPTIONS,PATCH,DELETE, POST' },
                    { key: 'Access-Control-Allow-Headers', value: 'X-CSRF-Token,X-Requested-Width, Origin,Content-Type' },
                ],
            },
        ];
    },
};