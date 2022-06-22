import { useEffect, useState } from "react";
import logo from "./logo.svg";
import "./App.css";

class WeatherForecast {
  date: string = "";
  temperatureC: number = 0;
  temperatureF: number = 0;
  summary: number = 0;
}

function App() {
  const [count, setCount] = useState(0);
  const [weather, setWeather] = useState<WeatherForecast[]>([]);

  useEffect(() => {
    const func = async () => {
      const w = (await fetch("/api/WeatherForecast").then((r) =>
        r.json()
      )) as WeatherForecast[];
      setWeather(w);
    };

    func();
  }, []);

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>Hello Vite + React!</p>
        <p>
          <button type="button" onClick={() => setCount((count) => count + 1)}>
            count is: {count}
          </button>
        </p>
        <p>
          Edit <code>App.tsx</code> and save to test HMR updates.
        </p>
        <p>
          <a
            className="App-link"
            href="https://reactjs.org"
            target="_blank"
            rel="noopener noreferrer"
          >
            Learn React
          </a>
          {" | "}
          <a
            className="App-link"
            href="https://vitejs.dev/guide/features.html"
            target="_blank"
            rel="noopener noreferrer"
          >
            Vite Docs
          </a>
        </p>
        <div>
          <ul>
            {weather.map((w, index) => {
              return (
                <li key={index}>
                  <span>{w.summary}</span>
                </li>
              );
            })}
          </ul>
        </div>
      </header>
    </div>
  );
}

export default App;
