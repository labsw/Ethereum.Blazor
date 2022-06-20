

export function promiseTimeout<T>(prom: Promise<T>, timeInMs: number, timeoutException: any) {
    let timer: any;
    return Promise.race([
        prom,
        new Promise((_r, rej) => timer = setTimeout(rej, timeInMs, timeoutException))
    ]).finally(() => clearTimeout(timer));
}

