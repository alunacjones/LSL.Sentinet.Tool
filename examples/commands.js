function trim(value) {
    return value.trim();
}

function trailingSlash(value) {
    return value.endsWith("/") ? value : `${value}/`;
}

function bold(value) {
    return `**!${value}!**`
}