import http from 'k6/http'
import { sleep } from 'k6'
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js"

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    //vus: 10,
    thresholds: {
        http_req_failed: ['rate<0.01'],
        http_req_duration: ['p(95)<500'],
    },
    stages:
        [
            { duration: '4m', target: 200 }, // Simulate ramp up
            { duration: '8m', target: 200 }, // Maintain 
            { duration: '4m', target: 0 }, // Ramp down
        ]
};

export default () => {
    http.get('http://REDACTED/api/Space/GetSpaceDetailsOnSpaceID/16', { headers: { "Accept": "*/*" } });
    sleep(1);
};

export function handleSummary(data) {
    return {
        "K6Reports/LoadTestSummary.html": htmlReport(data),
    };
}