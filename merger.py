from pathlib import Path

import pandas as pd


BASE_DIR = Path(__file__).resolve().parent
DATA_DIR = BASE_DIR / "Data"
APPS_FILE = DATA_DIR / "apps.csv"
NVDR_FILE = DATA_DIR / "nvdr.csv"
OUTPUT_FILE = DATA_DIR / "final merged.csv"


def normalize_dealer_code(series: pd.Series) -> pd.Series:
    cleaned = series.astype("string").str.strip()
    cleaned = cleaned.str.replace(r"\.0$", "", regex=True)
    cleaned = cleaned.str.replace(r"[^0-9A-Za-z]", "", regex=True)
    cleaned = cleaned.str.upper().str.lstrip("0")
    return cleaned


def normalize_sale_type(series: pd.Series) -> pd.Series:
    return series.astype("string").str.strip()


def normalize_period(series: pd.Series) -> pd.Series:
    parsed = pd.to_datetime(series, errors="coerce")
    return parsed.dt.to_period("M")


def format_period(series: pd.Series) -> pd.Series:
    as_period = pd.Series(series)
    as_timestamp = pd.Series(as_period.dt.to_timestamp(), index=as_period.index)
    return as_timestamp.map(lambda value: "" if pd.isna(value) else f"{value.month}/{value.day}/{value.year}")


def count_key_matches(left_df: pd.DataFrame, right_df: pd.DataFrame) -> int:
    left_keys = left_df[["DealerCode", "OEM Sale Period", "Sale Type"]].drop_duplicates()
    right_keys = right_df[["DealerCode", "OEM Sale Period", "Sale Type"]].drop_duplicates()
    matches = left_keys.merge(right_keys, on=["DealerCode", "OEM Sale Period", "Sale Type"], how="inner")
    return len(matches)


def best_month_offset(apps: pd.DataFrame, nvdr: pd.DataFrame, max_months: int = 24) -> tuple[int, int]:
    best_offset = 0
    best_matches = 0

    for offset in range(-max_months, max_months + 1):
        shifted = apps.copy()
        shifted["OEM Sale Period"] = shifted["OEM Sale Period"] + offset
        match_count = count_key_matches(nvdr, shifted)

        if match_count > best_matches:
            best_offset = offset
            best_matches = match_count

    return best_offset, best_matches


def load_apps() -> pd.DataFrame:
    apps = pd.read_csv(APPS_FILE)
    apps["DealerCode"] = normalize_dealer_code(apps["DealerCode"])
    apps["Sale Type"] = normalize_sale_type(apps["Sale Type"])
    apps["OEM Sale Period"] = normalize_period(apps["OEM Sale Period"])

    return apps[
        [
            "DealerCode",
            "OEM Sale Period",
            "Sale Type",
            "TotalApproved",
            "TotalDeclined",
            "TotalAppCount",
        ]
    ]


def load_nvdr() -> pd.DataFrame:
    nvdr = pd.read_csv(NVDR_FILE)
    nvdr["DealerCode"] = normalize_dealer_code(nvdr["DealerCode"])
    nvdr["Sale Type"] = normalize_sale_type(nvdr["Sale Type"])
    nvdr["OEM Sale Period"] = normalize_period(nvdr["OEM Sale Period"])
    return nvdr


def merge_data() -> pd.DataFrame:
    nvdr = load_nvdr()
    apps = load_apps()

    exact_matches = count_key_matches(nvdr, apps)
    applied_offset = 0

    if exact_matches == 0:
        offset, offset_matches = best_month_offset(apps, nvdr)
        if offset_matches > 0:
            apps = apps.copy()
            apps["OEM Sale Period"] = apps["OEM Sale Period"] + offset
            applied_offset = offset

    merged = nvdr.merge(
        apps,
        on=["DealerCode", "OEM Sale Period", "Sale Type"],
        how="left",
    )

    merged = merged.sort_values(
        by=["OEM Sale Period", "DealerCode", "Sale Type"],
        kind="stable",
    )

    for column in ["TotalApproved", "TotalDeclined", "TotalAppCount"]:
        merged[column] = pd.to_numeric(merged[column], errors="coerce").astype("Int64")

    merged["Booked Amount"] = pd.to_numeric(merged["Booked Amount"], errors="coerce").round(2)

    merged["OEM Sale Period"] = format_period(merged["OEM Sale Period"])
    if applied_offset != 0:
        print(f"No exact month matches found. Applied +{applied_offset} month offset to apps data.")
    return merged


def main() -> None:
    merged = merge_data()
    merged.to_csv(OUTPUT_FILE, index=False, float_format="%.2f")
    print(f"Merged file written to: {OUTPUT_FILE}")


if __name__ == "__main__":
    main()



