import { Link } from "@heroui/link";
import { Snippet } from "@heroui/snippet";
import { Code } from "@heroui/code";
import { button as buttonStyles } from "@heroui/theme";

import { siteConfig } from "../config/site";
import { title, subtitle } from "../components/primitives";
import { GithubIcon } from "../components/ui/icons";
import DefaultLayout from "../layouts/default";
import TaskLayout from "../components/layout/tasklayout";
import { useState } from "react";
import { Button } from "@heroui/button";

export default function IndexPage() {
  // right bar
  const [isOpen, setOpen] = useState(false);

  return (
    <DefaultLayout rightBar={<TaskLayout isOpen={isOpen} setOpen={setOpen} />}>
      <section className="flex flex-col items-center justify-center gap-4 py-8 md:py-10">
        <Button onPress={() => setOpen(!isOpen)}>open </Button>
        <div className="inline-block max-w-lg text-center justify-center">
          <span className={title()}>Make&nbsp;</span>
          <span className={title({ color: "violet" })}>beautiful&nbsp;</span>
          <br />
          <span className={title()}>
            websites regardless of your design experience.
          </span>
          <div className={subtitle({ class: "mt-4" })}>
            Beautiful, fast and modern React UI library.
          </div>
        </div>

        <div className="flex gap-3">
          <Link
            className={buttonStyles({
              color: "primary",
              radius: "full",
              variant: "shadow",
            })}
            href="/auth"
          >
            Login
          </Link>
          <Link
            isExternal
            className={buttonStyles({ variant: "bordered", radius: "full" })}
            href={siteConfig.links.github}
          >
            <GithubIcon size={20} />
            GitHub
          </Link>
        </div>

        <div className="mt-8">
          <Snippet hideCopyButton hideSymbol variant="bordered">
            <span>
              Get started by editing{" "}
              <Code color="primary">pages/index.tsx</Code>
            </span>
          </Snippet>
        </div>
      </section>
    </DefaultLayout>
  );
}

