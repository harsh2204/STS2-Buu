const RARITIES = ["All", "Basic", "Common", "Uncommon", "Rare", "Ancient"];

function el(tag, props = {}, children = []) {
  const node = document.createElement(tag);
  Object.entries(props).forEach(([k, v]) => {
    if (k === "class") node.className = v;
    else if (k === "html") node.innerHTML = v;
    else if (k.startsWith("on") && typeof v === "function") node.addEventListener(k.slice(2).toLowerCase(), v);
    else if (v === false || v == null) {
    } else if (k === "attrs") Object.entries(v).forEach(([a, val]) => node.setAttribute(a, val));
    else node[k] = v;
  });
  children.forEach((c) => {
    if (c == null) return;
    if (typeof c === "string") node.append(document.createTextNode(c));
    else node.append(c);
  });
  return node;
}

async function loadData() {
  const res = await fetch("./data.json", { cache: "no-store" });
  if (!res.ok) throw new Error(String(res.status));
  return res.json();
}

function renderCharacter(data) {
  const root = document.getElementById("character-root");
  const { names, images } = data.character;
  const imgs = [];

  if (images.select) {
    imgs.push(
      el("img", {
        class: "select-art",
        attrs: { src: images.select, alt: `${names.default} character select art`, loading: "lazy" },
      })
    );
  }
  if (images.merchant) {
    imgs.push(
      el("img", {
        class: "character-merchant",
        attrs: { src: images.merchant, alt: `${names.default} merchant`, loading: "lazy" },
      })
    );
  }

  const visual = el("div", { class: "character-visual" }, imgs.length ? imgs : [el("p", { class: "tile-placeholder" }, ["No images copied — run build_data.py"])]);

  const titleChildren = [];
  if (images.icon) {
    titleChildren.push(
      el("img", {
        class: "character-icon-inline",
        attrs: { src: images.icon, alt: "", loading: "lazy" },
      })
    );
  }
  titleChildren.push(el("h3", {}, [names.default]));

  const meta = el("div", { class: "character-meta" }, [
    el("div", { class: "character-title-row" }, titleChildren),
    el("div", { class: "name-row" }, [
      el("span", { class: "name-chip" }, [names.default]),
      el("span", { class: "name-chip name-chip--gold" }, [names.majin]),
      el("span", { class: "name-chip name-chip--ki" }, [names.super]),
    ]),
    el("p", { class: "section-lead" }, [data.meta.tagline]),
  ]);

  root.append(visual, meta);
}

function renderStances(data) {
  const root = document.getElementById("stances-root");
  root.replaceChildren(
    ...data.stances.map((s) =>
      el("article", { class: "stance-card" }, [el("h3", {}, [s.title]), el("p", {}, [s.description])])
    )
  );
}

function renderPowers(data) {
  const root = document.getElementById("powers-root");
  const input = document.getElementById("power-filter");
  const items = data.powers.map((p) => {
    const row = el("li", { attrs: { "data-q": `${p.title} ${p.description}`.toLowerCase() } }, [
      el("strong", {}, [p.title]),
      el("span", {}, [p.description]),
    ]);
    return row;
  });
  root.replaceChildren(...items);

  const apply = () => {
    const q = input.value.trim().toLowerCase();
    items.forEach((li) => {
      li.hidden = q && !li.dataset.q.includes(q);
    });
  };
  input.addEventListener("input", apply);
}

function renderRarityFilters(onChange) {
  const root = document.getElementById("rarity-filters");
  const buttons = RARITIES.map((r, i) =>
    el(
      "button",
      {
        type: "button",
        class: "filter-btn",
        attrs: { "aria-pressed": i === 0 ? "true" : "false", "data-rarity": r },
        onclick: () => {
          buttons.forEach((b) => b.setAttribute("aria-pressed", b.dataset.rarity === r ? "true" : "false"));
          onChange(r);
        },
      },
      [r]
    )
  );
  root.replaceChildren(...buttons);
}

function descParagraph(className, html) {
  const p = document.createElement("p");
  p.className = className;
  p.innerHTML = html || "…";
  return p;
}

function renderCards(data) {
  const root = document.getElementById("cards-root");
  const search = document.getElementById("card-filter");
  const upgradeChk = document.getElementById("card-show-upgraded");
  let currentRarity = "All";

  const tiles = data.cards.map((c) => {
    const figChildren = c.imageRel
      ? [
          el("img", {
            attrs: {
              src: c.imageRel,
              alt: c.title,
              loading: "lazy",
            },
          }),
        ]
      : [el("div", { class: "tile-placeholder" }, ["No portrait"])];

    const descEl = descParagraph("tile-desc", c.descriptionHtml);
    const plainUp = c.descriptionPlainUpgraded || c.descriptionPlain || "";
    const article = el(
      "article",
      {
        class: `tile ${c.useBigPortrait ? "tile--big" : ""}`,
        attrs: {
          "data-rarity": c.rarity,
          "data-q": `${c.title} ${c.descriptionPlain || ""} ${plainUp}`.toLowerCase(),
        },
      },
      [
        el("div", { class: "tile-fig" }, figChildren),
        el("div", { class: "tile-body" }, [
          el("h3", { class: "tile-title" }, [c.title]),
          el("div", { class: "tile-meta" }, [
            el("span", { class: `tag tag--${c.rarity}` }, [c.rarity]),
            el("span", { class: `tag tag--${c.type}` }, [c.type]),
          ]),
          descEl,
        ]),
      ]
    );
    return { article, descEl, c };
  });

  root.replaceChildren(...tiles.map((t) => t.article));

  const setUpgradeView = () => {
    const up = Boolean(upgradeChk?.checked);
    root.classList.toggle("card-grid--upgraded", up);
    tiles.forEach(({ descEl, c }) => {
      const html = up ? c.descriptionHtmlUpgraded || c.descriptionHtml : c.descriptionHtml;
      descEl.innerHTML = html || "…";
    });
  };

  upgradeChk?.addEventListener("change", setUpgradeView);

  const apply = () => {
    const q = search.value.trim().toLowerCase();
    tiles.forEach(({ article }) => {
      const rarityOk = currentRarity === "All" || article.dataset.rarity === currentRarity;
      const searchOk = !q || article.dataset.q.includes(q);
      article.classList.toggle("tile--hidden", !(rarityOk && searchOk));
    });
  };

  renderRarityFilters((r) => {
    currentRarity = r;
    apply();
  });
  search.addEventListener("input", apply);
  apply();
}

function renderRelics(data) {
  const root = document.getElementById("relics-root");
  const search = document.getElementById("relic-filter");

  const tiles = data.relics.map((r) => {
    const figChildren = r.imageRel
      ? [el("img", { attrs: { src: r.imageRel, alt: r.title, loading: "lazy" } })]
      : [el("div", { class: "tile-placeholder" }, ["No icon"])];

    const flavorEl = r.flavorHtml ? descParagraph("relic-flavor", r.flavorHtml) : null;

    return el(
      "article",
      {
        class: "tile relic-tile",
        attrs: {
          "data-q": `${r.title} ${r.descriptionPlain || ""} ${r.flavorPlain || ""}`.toLowerCase(),
        },
      },
      [
        el("div", { class: "tile-fig" }, figChildren),
        el("div", { class: "tile-body" }, [
          el("h3", { class: "tile-title" }, [r.title]),
          descParagraph("tile-desc", r.descriptionHtml),
          ...(flavorEl ? [flavorEl] : []),
        ]),
      ]
    );
  });

  root.replaceChildren(...tiles);

  const apply = () => {
    const q = search.value.trim().toLowerCase();
    tiles.forEach((tile) => {
      const ok = !q || tile.dataset.q.includes(q);
      tile.classList.toggle("tile--hidden", !ok);
    });
  };
  search.addEventListener("input", apply);
}

function main() {
  loadData()
    .then((data) => {
      document.getElementById("load-error").hidden = true;
      document.getElementById("hero-name").textContent = data.meta.name;
      document.getElementById("hero-tagline").textContent = data.meta.tagline;
      renderCharacter(data);
      renderStances(data);
      renderPowers(data);
      renderCards(data);
      renderRelics(data);
    })
    .catch(() => {
      document.getElementById("load-error").hidden = false;
    });
}

main();
