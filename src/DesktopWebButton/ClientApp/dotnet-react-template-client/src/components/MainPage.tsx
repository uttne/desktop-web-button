import {
  Card,
  CardActionArea,
  CardContent,
  Grid,
  Paper,
  Stack,
} from "@mui/material";
import { ReactNode, useEffect, useState } from "react";
import "./Page.css";
import { css } from "@emotion/react";

const rootPageStyle = css({
  backgroundColor: "#e8e8e8",
  "> div": { margin: 10 },
});

class ItemProps {
  name: string = "";
}

function Item(props: ItemProps) {
  const action = async () => {
    await fetch(`/api/Button?name=${props.name}`, { method: "POST" });
  };
  return (
    <Card>
      <CardActionArea onClick={action}>
        <CardContent>{props.name}</CardContent>
      </CardActionArea>
    </Card>
  );
}

class ButtonData {
  name: string = "";
}

export default function MainPage() {
  const arr = new Array(10).fill(undefined).map(() => "aa");

  const [buttons, setButtons] = useState<ButtonData[]>([]);

  useEffect(() => {
    const func = async () => {
      const b = (await fetch("/api/Button").then((r) =>
        r.json()
      )) as ButtonData[];
      setButtons(b);
    };

    func();
  }, []);

  return (
    <div className="page-root" css={rootPageStyle}>
      <div>
        <Grid container spacing={1}>
          {buttons.map((x, index) => {
            return (
              <Grid key={index} item xs={3} sm={2}>
                <Item name={x.name}></Item>
              </Grid>
            );
          })}
        </Grid>
      </div>
    </div>
  );
}
