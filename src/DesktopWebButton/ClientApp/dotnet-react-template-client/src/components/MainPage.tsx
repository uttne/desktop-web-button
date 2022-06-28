import {
  Card,
  CardActionArea,
  CardContent,
  Grid,
  Paper,
  Stack,
  Typography,
} from "@mui/material";
import { ReactNode, useEffect, useState } from "react";
import "./Page.css";
import { css } from "@emotion/react";

const rootPageStyle = css({
  backgroundColor: "#e8e8e8",
  "> div": { margin: 10 },
});

class ItemProps {
  button?: ButtonData;
}

function Item(props: ItemProps) {
  const action = async () => {
    await fetch(`/api/Button?name=${props.button?.name}`, { method: "POST" });
  };
  return (
    <Card>
      <CardActionArea onClick={action}>
        <CardContent>
          <Typography gutterBottom variant="h6">
            {props.button?.display ?? props.button?.name}
          </Typography>
          <Typography variant="body2" color="text.secondary">
            {props.button?.description}
          </Typography>
        </CardContent>
      </CardActionArea>
    </Card>
  );
}

class ButtonData {
  kind: string = "";
  name: string = "";
  display?: string;
  description?: string;
}

export default function MainPage() {
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
              <Grid key={index} item xs={6} sm={4} md={3}>
                <Item button={x}></Item>
              </Grid>
            );
          })}
        </Grid>
      </div>
    </div>
  );
}
